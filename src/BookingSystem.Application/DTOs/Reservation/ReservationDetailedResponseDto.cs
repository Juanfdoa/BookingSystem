using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs.Reservation
{
    public class ReservationDetailedResponseDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public ReservationStatus Status { get; set; }
        public decimal TotalCost { get; set; }

        public IEnumerable<GuestResponseDto> Guests { get; set; } = [];
        public EmergencyContactResponseDto EmergencyContact { get; set; } = default!;
    }
}
