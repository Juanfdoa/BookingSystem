using BookingSystem.Application.DTOs.Room;
using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Application.Wrapper;
using BookingSystem.Domain.Common;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Enums;
using BookingSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdWithHotelAsync(Guid id)
        {
            return await _context.Rooms
                .Include(x => x.Hotel)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResult<Room>> GetFilteredAsync(Guid hotelId, RoomFilter filter, CancellationToken cancellationToken = default)
        {
            IQueryable<Room> query = _context.Rooms
                .Where(x => x.HotelId == hotelId);

            if (!string.IsNullOrWhiteSpace(filter.Type))
            {
                query = query.Where(x =>
                    EF.Functions.ILike(x.Type.ToString(), $"%{filter.Type}%"));
            }

            if (filter.Capacity.HasValue)
            {
                query = query.Where(x => x.Capacity == filter.Capacity.Value);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(x => x.BasePrice >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(x => x.BasePrice <= filter.MaxPrice.Value);
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            var rooms = await query
                .OrderBy(x => x.Number)
                .ToListAsync(cancellationToken);

            return new PagedResult<Room>
            {
                Items = rooms,
                TotalRecords = totalRecords
            };
        }

        public async Task<PagedResult<Room>> SearchAvailableAsync(AvailabilityFilter filter, Pagination pagination, CancellationToken cancellationToken = default)
        {
            var checkInUtc = DateTime.SpecifyKind(filter.CheckIn, DateTimeKind.Utc);
            var checkOutUtc = DateTime.SpecifyKind(filter.CheckOut, DateTimeKind.Utc);

            var pageNumber = Math.Max(1, pagination.PageNumber);
            var pageSize = Math.Clamp(pagination.PageSize, 1, 100);

            var query = _context.Rooms
                .AsNoTracking()
                .Include(x => x.Hotel)
                .Where(x =>
                    x.IsActive &&
                    x.Hotel.IsActive &&
                    x.Hotel.City == filter.City &&
                    x.Capacity >= filter.Guests &&
                    !_context.Reservations.Any(r =>
                        r.RoomId == x.Id &&
                        r.Status != ReservationStatus.Cancelled &&
                        checkInUtc < r.CheckOut &&
                        checkOutUtc > r.CheckIn));

            var totalRecords = await query.CountAsync(cancellationToken);

            var rooms = await query
                .OrderBy(x => x.Hotel.Name)
                .ThenBy(x => x.Number)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Room>
            {
                Items = rooms,
                TotalRecords = totalRecords
            };
        }

        public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            var checkInUtc = DateTime.SpecifyKind(checkIn, DateTimeKind.Utc);
            var checkOutUtc = DateTime.SpecifyKind(checkOut, DateTimeKind.Utc);

            return !await _context.Reservations.AnyAsync(x =>
                x.RoomId == roomId &&
                x.Status != ReservationStatus.Cancelled &&
                checkInUtc < x.CheckOut &&
                checkOutUtc > x.CheckIn);
        }
    }
}