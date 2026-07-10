using BookingSystem.Application.DTOs.Reservation;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Mapping
{
    public static class ReservationMapper
    {
        public static ReservationResponseDto ToResponseDto(Reservation reservation)
        {
            return new ReservationResponseDto
            {
                Id = reservation.Id,
                RoomId = reservation.Room.Id,
                HotelName = reservation.Room.Hotel.Name,
                RoomNumber = reservation.Room.Number,
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                Status = reservation.Status,
                Guests = reservation.Guests.Count
            };
        }

        public static ReservationDetailedResponseDto ToDetailedResponseDto(Reservation reservation)
        {
            return new ReservationDetailedResponseDto
            {
                Id = reservation.Id,
                RoomId = reservation.Room.Id,
                HotelName = reservation.Room.Hotel.Name,
                RoomNumber = reservation.Room.Number,
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                Status = reservation.Status,
                TotalCost = reservation.TotalCost,

                Guests = reservation.Guests
                    .Select(ToGuestResponseDto)
                    .ToList(),

                EmergencyContact = ToCEmergencyContactResponseDto(
                    reservation.EmergencyContact)
            };
        }

        private static GuestResponseDto ToGuestResponseDto(Guest guest)
        {
            return new GuestResponseDto
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Gender = guest.Gender,
                DocumentType = guest.DocumentType,
                DocumentNumber = guest.DocumentNumber,
                Email = guest.Email,
                Phone = guest.Phone
            };
        }

        private static EmergencyContactResponseDto ToCEmergencyContactResponseDto(EmergencyContact contact)
        {
            return new EmergencyContactResponseDto
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Phone = contact.Phone
            };
        }

    }
}
