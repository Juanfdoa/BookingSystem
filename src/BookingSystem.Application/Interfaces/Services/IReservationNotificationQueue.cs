using BookingSystem.Application.DTOs.Reservation;

namespace BookingSystem.Application.Interfaces.Services
{
    public interface IReservationNotificationQueue
    {
        ValueTask EnqueueAsync(ReservationConfirmationNotification notification);
    }
}
