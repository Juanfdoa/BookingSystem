namespace BookingSystem.Domain.Common
{
    public class PagedResult<T> 
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalRecords { get; set; }
    }
}
