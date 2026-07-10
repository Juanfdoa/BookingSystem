namespace BookingSystem.Application.Interfaces.Providers
{
    public interface IEmailService
    {
        Task SendAsync(string to, IEnumerable<string>? cc, string subject, string body);
        Task SendTemplateAsync(string to, IEnumerable<string>? cc, string subject, string templateName, Dictionary<string, object?> placeholders);
        Task SendReservationConfirmationAsync(IEnumerable<string> guestEmails, string subject, string templateName, Dictionary<string, object?> placeholders);
    }
}
