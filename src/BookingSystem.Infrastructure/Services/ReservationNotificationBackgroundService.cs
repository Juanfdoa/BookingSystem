using BookingSystem.Application.Interfaces.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookingSystem.Infrastructure.Services
{
    public sealed class ReservationNotificationBackgroundService : BackgroundService
    {
        private readonly ReservationNotificationQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReservationNotificationBackgroundService> _logger;

        public ReservationNotificationBackgroundService(
            ReservationNotificationQueue queue,
            IServiceScopeFactory scopeFactory,
            ILogger<ReservationNotificationBackgroundService> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var notification in _queue.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    await emailService.SendReservationConfirmationAsync(
                        notification.GuestEmails,
                        notification.Subject,
                        notification.TemplateName,
                        notification.Placeholders);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Failed to send reservation confirmation notification.");
                }
            }
        }
    }
}
