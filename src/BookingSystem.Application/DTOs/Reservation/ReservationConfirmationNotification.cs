namespace BookingSystem.Application.DTOs.Reservation
{
    public sealed class ReservationConfirmationNotification
    {
        public IReadOnlyCollection<string> GuestEmails { get; init; } = [];
        public string Subject { get; init; } = string.Empty;
        public string TemplateName { get; init; } = string.Empty;
        public Dictionary<string, object?> Placeholders { get; init; } = [];
    }
}
