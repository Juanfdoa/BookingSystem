using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs.Reservation
{
    public class ReservationResponseDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
