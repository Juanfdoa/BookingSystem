using BookingSystem.Application.DTOs.Hotel;
using BookingSystem.Application.Exceptions;
using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Application.Interfaces.Services;
using BookingSystem.Application.Mapping;
using BookingSystem.Application.Wrapper;

namespace BookingSystem.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HotelService(IHotelRepository hotelRepository,
            IUnitOfWork unitOfWork)
        {
            _hotelRepository = hotelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<HotelResponseDto>>> GetAllAsync(HotelFilter filter, Pagination pagination)
        {
            var result = await _hotelRepository.GetFilteredAsync(filter, pagination);

            return new ApiResponse<IEnumerable<HotelResponseDto>>
            {
                Success = true,
                Data = result.Items.Select(HotelMapper.ToResponseDto),
                Pagination = new PaginationResponse
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRecords = result.TotalRecords
                }
            };
        }

        public async Task<ApiResponse<HotelDetailedResponseDto?>> GetByIdAsync(Guid id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("The requested resource was not found.");

            return new ApiResponse<HotelDetailedResponseDto?>
            {
                Data = HotelMapper.ToDetailedResponseDto(hotel)
            };
        }

        public async Task<ApiResponse<HotelDetailedResponseDto>> CreateAsync(HotelRequestDto request)
        {
            var hotel = HotelMapper.ToEntity(request);
            hotel = await _hotelRepository.AddAsync(hotel);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<HotelDetailedResponseDto>
            {
                Data = HotelMapper.ToDetailedResponseDto(hotel)
            };
        }

        public async Task UpdateAsync(Guid id, HotelRequestDto request)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException("The requested resource was not found.");

            HotelMapper.UpdateEntity(request, hotel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("The requested resource was not found.");

            hotel.SoftDelete();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStatus(Guid id, bool isActive)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("The requested resource was not found.");

            hotel.IsActive = isActive;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
