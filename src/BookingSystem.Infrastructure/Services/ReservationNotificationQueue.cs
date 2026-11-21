using System.Threading.Channels;
using BookingSystem.Application.DTOs.Reservation;
using BookingSystem.Application.Interfaces.Services;

namespace BookingSystem.Infrastructure.Services
{
    public sealed class ReservationNotificationQueue : IReservationNotificationQueue
    {
        private readonly Channel<ReservationConfirmationNotification> _channel =
            Channel.CreateBounded<ReservationConfirmationNotification>(
                new BoundedChannelOptions(500)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = true,
                    SingleWriter = false
                });

        public ValueTask EnqueueAsync(ReservationConfirmationNotification notification)
        {
            return _channel.Writer.WriteAsync(notification);
        }

        public IAsyncEnumerable<ReservationConfirmationNotification> ReadAllAsync(
            CancellationToken cancellationToken)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}
