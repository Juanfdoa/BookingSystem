using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs.Room
{
    public class RoomResponseDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = default!;
        public RoomType Type { get; set; } = default!;
        public bool IsActive { get; set; }
        public int Capacity { get; set; } = default!;
        public decimal BasePrice { get; set; }
    }
}
