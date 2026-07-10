using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs.Room
{
    public class AvailableRoomResponseDto
    {
        public Guid RoomId { get; set; }
        public Guid HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public RoomType Type { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
