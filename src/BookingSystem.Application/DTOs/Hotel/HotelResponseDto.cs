namespace BookingSystem.Application.DTOs.Hotel
{
    public class HotelResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string City { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
