using BookingSystem.Domain.Common;
using BookingSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    [Table("reservations")]
    public class Reservation : BaseEntity
    {
        public Guid RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public ReservationStatus Status { get; set; }
        public decimal TotalCost { get; set; }

        public Room Room { get; set; } = default!;
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
        public EmergencyContact EmergencyContact { get; set; } = null!;
    }
}
