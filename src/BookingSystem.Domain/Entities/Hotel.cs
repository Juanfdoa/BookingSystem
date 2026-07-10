using BookingSystem.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    [Table("hotels")]
    public class Hotel : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string City { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        public virtual void SoftDelete()
        {
            IsActive = false;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
