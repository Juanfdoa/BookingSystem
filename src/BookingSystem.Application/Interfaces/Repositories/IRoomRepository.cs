using BookingSystem.Application.DTOs.Room;
using BookingSystem.Domain.Common;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces.Repositories
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room?> GetByIdWithHotelAsync(Guid id);
        Task<PagedResult<Room>> GetFilteredAsync(Guid hotelId, RoomFilter filter, CancellationToken cancellationToken = default);
        Task<IEnumerable<Room>> SearchAvailableAsync(AvailabilityFilter filter, CancellationToken cancellationToken = default);
        Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut);
    }
}
