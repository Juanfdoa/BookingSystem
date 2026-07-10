using BookingSystem.Application.DTOs.Hotel;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Common;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces.Repositories
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<PagedResult<Hotel>> GetFilteredAsync(HotelFilter filter, Pagination pagination, CancellationToken cancellationToken = default);
    }
}
