using BookingSystem.Application.DTOs.Room;
using BookingSystem.Application.Wrapper;

namespace BookingSystem.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task<ApiResponse<IEnumerable<RoomResponseDto>>> GetAllAsync(Guid hotelId, RoomFilter filter);
        Task<ApiResponse<RoomDetailedResponseDto?>> GetByIdAsync(Guid id);
        Task<ApiResponse<RoomDetailedResponseDto>> CreateAsync(RoomRequestDto request);
        Task UpdateAsync(Guid id, RoomUpdateRequestDto request);
        Task DeleteAsync(Guid id);
        Task<ApiResponse<IEnumerable<AvailableRoomResponseDto>>> SearchAvailableAsync(AvailabilityFilter filter, CancellationToken cancellationToken = default);
        Task UpdateStatus(Guid id, bool isActive);
    }
}
