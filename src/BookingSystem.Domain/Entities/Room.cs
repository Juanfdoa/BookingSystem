using BookingSystem.Domain.Common;
using BookingSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    [Table("rooms")]
    public class Room : BaseEntity
    {
        public Guid HotelId { get; set; }
        public string Number { get; set; } = string.Empty;
        public RoomType Type { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public Hotel Hotel { get; set; } = null!;

        public virtual void SoftDelete()
        {
            IsActive = false;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
