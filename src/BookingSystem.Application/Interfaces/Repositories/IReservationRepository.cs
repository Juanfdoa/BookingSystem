using BookingSystem.Application.DTOs.Reservation;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Common;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces.Repositories
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        Task<PagedResult<Reservation>> GetFilteredAsync(ReservationFilter filter, Pagination pagination,CancellationToken cancellationToken = default);
        Task<Reservation?> GetByIdWithIncludesAsync(Guid id,CancellationToken cancellationToken = default);
    }
}
