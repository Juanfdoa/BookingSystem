using BookingSystem.Application.DTOs.Hotel;
using BookingSystem.Application.Wrapper;

namespace BookingSystem.Application.Interfaces.Services
{
    public interface IHotelService
    {
        Task<ApiResponse<IEnumerable<HotelResponseDto>>> GetAllAsync(HotelFilter filter, Pagination pagination);
        Task<ApiResponse<HotelDetailedResponseDto?>> GetByIdAsync(Guid id);
        Task<ApiResponse<HotelDetailedResponseDto>> CreateAsync(HotelRequestDto request);
        Task UpdateAsync(Guid id, HotelRequestDto request);
        Task DeleteAsync(Guid id);
        Task UpdateStatus(Guid id, bool isActive);
    }
}
