namespace BookingSystem.Application.DTOs.Room
{
    public class AvailabilityFilter
    {
        public string City { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
    }
}
