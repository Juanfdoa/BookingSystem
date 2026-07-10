using BookingSystem.Application.Exceptions;
using BookingSystem.Application.Interfaces.Providers;
using Scriban;
using Scriban.Runtime;
using System.Net;
using System.Net.Mail;

namespace BookingSystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string to, IEnumerable<string>? cc, string subject, string body)
        {
            var smtp = new SmtpClient(Environment.GetEnvironmentVariable("EMAIL_SMTP") ?? throw new Exception("EMAIL_SMTP not configured"),
            int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT")! ?? throw new Exception("EMAIL_PORT not configured")))
            {
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? throw new Exception("EMAIL_USERNAME not configured"),
                    Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? throw new Exception("EMAIL_PASSWORD not configured")),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(
                Environment.GetEnvironmentVariable("EMAIL_USERNAME")!),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            if (cc != null)
            {
                foreach (var email in cc.Distinct())
                {
                    message.CC.Add(email);
                }
            }

            await smtp.SendMailAsync(message);
        }

        public async Task SendTemplateAsync(string to,IEnumerable<string>? cc, string subject, string templateName, Dictionary<string, object?> placeholders)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Templates", $"{templateName}.html");

            if (!File.Exists(path))
                throw new FileNotFoundException(
                    $"Email template '{templateName}' was not found.");

            var html = await File.ReadAllTextAsync(path);

            var template = Template.Parse(html);

            if (template.HasErrors)
                throw new Exception(template.Messages.First().Message);

            var scriptObject = new ScriptObject();

            foreach (var placeholder in placeholders)
            {
                scriptObject.Add(placeholder.Key, placeholder.Value);
            }

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);

            var body = template.Render(context);

            await SendAsync(to, cc, subject, body);
        }

        public async Task SendReservationConfirmationAsync(IEnumerable<string> guestEmails,string subject,string templateName,Dictionary<string, object?> placeholders)
        {
            var emails = guestEmails
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (!emails.Any())
                throw new BusinessException("No guest email addresses were provided.");

            var to = emails.First();

            var cc = emails.Skip(1).ToList();

            await SendTemplateAsync(to, cc, subject, templateName, placeholders);
        }
    }
}
