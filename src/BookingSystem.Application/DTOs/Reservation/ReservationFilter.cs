using BookingSystem.Domain.Enums;

namespace BookingSystem.Application.DTOs.Reservation
{
    public class ReservationFilter
    {
        public Guid? HotelId { get; set; }
        public ReservationStatus? Status { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}
