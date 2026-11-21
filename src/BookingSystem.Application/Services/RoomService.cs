using BookingSystem.Application.DTOs.Room;
using BookingSystem.Application.Exceptions;
using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Application.Interfaces.Services;
using BookingSystem.Application.Mapping;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IRoomRepository roomRepository,
            IHotelRepository hotelRepository,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<RoomResponseDto>>> GetAllAsync(Guid hotelId, RoomFilter filter)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId)
                ?? throw new NotFoundException("The requested resource was not found.");

            if (!hotel.IsActive)
                throw new BusinessException("The hotel is temporarily unavailable.");

            var result = await _roomRepository.GetFilteredAsync(hotelId, filter);

            return new ApiResponse<IEnumerable<RoomResponseDto>>
            {
                Success = true,
                Data = result.Items.Select(RoomMapper.ToResponseDto),
                Pagination = new PaginationResponse
                {
                    TotalRecords = result.TotalRecords
                }
            };
        }

        public async Task<ApiResponse<RoomDetailedResponseDto?>> GetByIdAsync(Guid id)
        {
            var room = await _roomRepository.GetByIdAsync(id)
               ?? throw new NotFoundException("The requested resource was not found.");

            return new ApiResponse<RoomDetailedResponseDto?>
            {
                Data = RoomMapper.ToDetailedResponseDto(room)
            };
        }

        public async Task<ApiResponse<RoomDetailedResponseDto>> CreateAsync(RoomRequestDto request)
        {
            var hotel = await _hotelRepository.GetByIdAsync(request.HotelId)
                ?? throw new NotFoundException("The specified hotel was not found.");

            if (!hotel.IsActive)
                throw new BusinessException("Cannot create a room because the hotel is inactive.");

            var room = RoomMapper.ToEntity(request);
            room = await _roomRepository.AddAsync(room);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<RoomDetailedResponseDto>
            {
                Data = RoomMapper.ToDetailedResponseDto(room)
            };
        }

        public async Task UpdateAsync(Guid id, RoomUpdateRequestDto request)
        {
            var room = await _roomRepository.GetByIdAsync(id)
               ?? throw new NotFoundException("The requested resource was not found.");

            RoomMapper.UpdateEntity(request, room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var room = await _roomRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("The requested resource was not found.");

            room.SoftDelete();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ApiResponse<IEnumerable<AvailableRoomResponseDto>>> SearchAvailableAsync(AvailabilityFilter filter, Pagination pagination, CancellationToken cancellationToken = default)
        {
            var result = await _roomRepository.SearchAvailableAsync(filter, pagination, cancellationToken);

            var pageNumber = Math.Max(1, pagination.PageNumber);
            var pageSize = Math.Clamp(pagination.PageSize, 1, 100);

            return new ApiResponse<IEnumerable<AvailableRoomResponseDto>>
            {
                Success = true,
                Data = result.Items.Select(RoomMapper.ToAvailableResponseDto),
                Pagination = new PaginationResponse
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = result.TotalRecords
                }
            };
        }

        public async Task UpdateStatus(Guid id, bool isActive)
        {
            var room = await _roomRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("The requested resource was not found.");

            room.IsActive = isActive;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}