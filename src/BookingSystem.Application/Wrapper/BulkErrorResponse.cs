using System.Text.Json.Serialization;

namespace BookingSystem.Application.Wrapper
{
    public class BulkErrorResponse
    {
        [JsonPropertyName("item")]
        public object? Item { get; set; }

        [JsonPropertyName("errors")]
        public List<string>? Errors { get; set; }
    }
}
