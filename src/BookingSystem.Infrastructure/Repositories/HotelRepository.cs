using BookingSystem.Application.DTOs.Hotel;
using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Common;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<Hotel>> GetFilteredAsync(HotelFilter filter, Pagination pagination, CancellationToken cancellationToken = default)
        {
            IQueryable<Hotel> query = _context.Hotels;

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => EF.Functions.ILike(x.Name, $"%{filter.Name}%"));

            if (!string.IsNullOrWhiteSpace(filter.City))
                query = query.Where(x => EF.Functions.ILike(x.City, $"%{filter.City}%"));

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive);

            var totalRecords = await query.CountAsync(cancellationToken);

            var hotels = await query
                .OrderBy(x => x.Name)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Hotel>
            {
                Items = hotels,
                TotalRecords = totalRecords
            };
        }
    }
}
