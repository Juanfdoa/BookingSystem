using BookingSystem.Application.Interfaces.Repositories;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Context;

namespace BookingSystem.Infrastructure.Repositories
{
    public class EmergencyContactRepository : BaseRepository<EmergencyContact>, IEmergencyContactRepository
    {
        public EmergencyContactRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
