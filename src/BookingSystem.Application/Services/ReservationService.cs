using BookingSystem.Application.Constants;
using BookingSystem.Application.DTOs.Reservation;
using BookingSystem.Application.Exceptions;
using BookingSystem.Application.Interfaces.Providers;
using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Application.Interfaces.Services;
using BookingSystem.Application.Mapping;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IEmergencyContactRepository _emergencyContactRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IReservationRepository reservationRepository, 
            IRoomRepository roomRepository,
            IGuestRepository guestRepository,
            IEmergencyContactRepository emergencyContactRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
            _emergencyContactRepository = emergencyContactRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<ReservationResponseDto>>> GetAllAsync(ReservationFilter filter, Pagination pagination)
        {
            var result = await _reservationRepository.GetFilteredAsync(filter,pagination);

            return new ApiResponse<IEnumerable<ReservationResponseDto>>
            {
                Success = true,
                Data = result.Items.Select(ReservationMapper.ToResponseDto),
                Pagination = new PaginationResponse
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRecords = result.TotalRecords
                }
            };
        }

        public async Task<ApiResponse<ReservationDetailedResponseDto>> GetByIdAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetByIdWithIncludesAsync(id)
                    ?? throw new NotFoundException("The requested resource was not found.");

            return new ApiResponse<ReservationDetailedResponseDto>
            {
                Data = reservation != null ? ReservationMapper.ToDetailedResponseDto(reservation) : null
            };
        }

        public async Task<ApiResponse<ReservationDetailedResponseDto>> CreateAsync(ReservationRequestDto request)
        {
            // 1. Buscar habitación
            var room = await _roomRepository.GetByIdWithHotelAsync(request.RoomId);

            if (room is null)
                throw new NotFoundException("Room not found.");

            // 2. Validar estado
            if (!room.IsActive)
                throw new BusinessException("Room is disabled.");

            if (!room.Hotel.IsActive)
                throw new BusinessException("Hotel is disabled.");

            // 3. Validar huéspedes
            if (request.Guests.Count > room.Capacity)
                throw new BusinessException("The number of guests exceeds the room capacity.");

            // 4. Validar disponibilidad
            var available = await _roomRepository.IsRoomAvailableAsync(
                room.Id,
                request.CheckIn,
                request.CheckOut);

            if (!available)
                throw new BusinessException("Room is not available for the selected dates.");

            // 5. Calcular costo
            var nights = (request.CheckOut.Date - request.CheckIn.Date).Days;

            var totalCost = nights * (room.BasePrice + room.Taxes);

            // 6. Crear reserva
            var reservation = new Reservation
            {
                RoomId = room.Id,
                CheckIn = DateTime.SpecifyKind(request.CheckIn, DateTimeKind.Utc),
                CheckOut = DateTime.SpecifyKind(request.CheckOut, DateTimeKind.Utc),
                Status = ReservationStatus.Confirmed,
                TotalCost = totalCost
            };

            await _reservationRepository.AddAsync(reservation);

            // 7. Crear huéspedes
            var guests = request.Guests.Select(x => new Guest
            {
                ReservationId = reservation.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                DocumentType = x.DocumentType,
                DocumentNumber = x.DocumentNumber,
                Email = x.Email,
                Phone = x.Phone
            }).ToList();

            await _guestRepository.AddRangeAsync(guests);

            // 8. Contacto de emergencia
            await _emergencyContactRepository.AddAsync(new EmergencyContact
            {
                ReservationId = reservation.Id,
                FirstName = request.EmergencyContact.FirstName,
                LastName = request.EmergencyContact.LastName,
                Phone = request.EmergencyContact.Phone
            });

            await _unitOfWork.SaveChangesAsync();

            // 9. Enviar correo
            try
            {
                await SendReservationConfirmationEmailAsync(reservation, room, guests);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send confirmation email: {ex.Message}");
            }

            var createdReservation = await _reservationRepository.GetByIdWithIncludesAsync(reservation.Id);

            return new ApiResponse<ReservationDetailedResponseDto>
            {
                Data = ReservationMapper.ToDetailedResponseDto(createdReservation!)
            };
        }

        public async Task UpdateReservationStatus(Guid id, ReservationStatus status)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id)
                     ?? throw new NotFoundException("The requested resource was not found.");

            if (reservation.Status == ReservationStatus.Cancelled)
            {
                throw new BusinessException("A cancelled reservation cannot be modified.");
            }

            reservation.Status = status;
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task SendReservationConfirmationEmailAsync(Reservation reservation,Room room,List<Guest> guests)
        {
            var emails = guests
                .Select(x => x.Email)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            if (!emails.Any())
                return;

            await _emailService.SendTemplateAsync(
                to: emails.First(),
                subject: "Reservation Confirmed",
                templateName: EmailTemplates.ConfirmReservation,
                placeholders: new Dictionary<string, object?>
                {
                    ["Title"] = "Reservation Confirmed",
                    ["ApplicationName"] = "Booking System",
                    ["Year"] = DateTime.UtcNow.Year,

                    ["FirstName"] = guests.First().FirstName,

                    ["ReservationId"] = reservation.Id,

                    ["HotelName"] = room.Hotel.Name,
                    ["City"] = room.Hotel.City,
                    ["Address"] = room.Hotel.Address,

                    ["RoomNumber"] = room.Number,
                    ["RoomType"] = room.Type.ToString(),

                    ["CheckIn"] = reservation.CheckIn.ToString("dd/MM/yyyy"),
                    ["CheckOut"] = reservation.CheckOut.ToString("dd/MM/yyyy"),

                    ["Guests"] = guests.Count,
                    ["TotalCost"] = reservation.TotalCost.ToString("C"),
                    ["Status"] = reservation.Status.ToString()
                },
                cc: emails.Skip(1));
        }
    }
}
