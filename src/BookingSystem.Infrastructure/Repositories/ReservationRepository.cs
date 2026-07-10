using BookingSystem.Application.DTOs.Reservation;
using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Common;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<Reservation>> GetFilteredAsync(ReservationFilter filter, Pagination pagination, CancellationToken cancellationToken = default)
        {
            IQueryable<Reservation> query = _context.Reservations
                .Include(x => x.Room)
                .ThenInclude(x => x.Hotel)
                .Include(x => x.Guests);

            if (filter.HotelId.HasValue)
            {
                query = query.Where(x => x.Room.HotelId == filter.HotelId);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(x => x.Status == filter.Status);
            }

            if (filter.CheckIn.HasValue)
            {
                var checkInUtc = DateTime.SpecifyKind((DateTime)filter.CheckIn, DateTimeKind.Utc);
                query = query.Where(x => x.CheckIn.Date >= checkInUtc.Date);
            }

            if (filter.CheckOut.HasValue)
            {
                var checkOutUtc = DateTime.SpecifyKind((DateTime)filter.CheckOut, DateTimeKind.Utc);
                query = query.Where(x => x.CheckOut.Date <= checkOutUtc.Date);
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            var reservations = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Reservation>
            {
                Items = reservations,
                TotalRecords = totalRecords
            };
        }

        public async Task<Reservation?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Reservations
                .AsNoTracking()
                .Include(x => x.Room).ThenInclude(x => x.Hotel)
                .Include(x => x.Guests)
                .Include(x => x.EmergencyContact)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
