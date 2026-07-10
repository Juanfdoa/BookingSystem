using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs.Room
{
    public class RoomUpdateRequestDto
    {
        public string Number { get; set; } = default!;
        public RoomType Type { get; set; } = default!;
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public string Location { get; set; } = default!;
    }
}
