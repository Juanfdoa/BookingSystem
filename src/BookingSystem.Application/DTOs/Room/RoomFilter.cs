namespace BookingSystem.Application.DTOs.Room
{
    public class RoomFilter
    {
        public string? Type { get; set; }
        public int? Capacity { get; set; }
        public bool? IsActive { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
