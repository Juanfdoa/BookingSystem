using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Context;

namespace BookingSystem.Infrastructure.Repositories
{
    public class GuestRepository : BaseRepository<Guest>, IGuestRepository
    {
        public GuestRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
