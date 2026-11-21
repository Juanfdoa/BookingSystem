using BookingSystem.Application.DTOs.Room;
using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Common;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces.Repositories
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room?> GetByIdWithHotelAsync(Guid id);
        Task<PagedResult<Room>> GetFilteredAsync(Guid hotelId, RoomFilter filter, CancellationToken cancellationToken = default);
        Task<PagedResult<Room>> SearchAvailableAsync(AvailabilityFilter filter, Pagination pagination, CancellationToken cancellationToken = default);
        Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut);
    }
}