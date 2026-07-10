using BookingSystem.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    [Table("emergency_contacts")]
    public class EmergencyContact : BaseEntity
    {
        public Guid ReservationId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public Reservation Reservation { get; set; } = null!;
    }
}
