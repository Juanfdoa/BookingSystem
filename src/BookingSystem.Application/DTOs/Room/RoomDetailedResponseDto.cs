using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs.Room
{
    public class RoomDetailedResponseDto
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public string Number { get; set; } = default!;
        public RoomType Type { get; set; } = default!;
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public string Location { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
