namespace BookingSystem.Application.DTOs.Reservation
{
    public class ReservationRequestDto
    {
        public Guid RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public List<GuestRequestDto> Guests { get; set; } = [];
        public EmergencyContactRequestDto EmergencyContact { get; set; } = default!;
    }
}
