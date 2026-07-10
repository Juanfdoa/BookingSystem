using BookingSystem.Application.DTOs.Reservation;
using FluentValidation;

namespace BookingSystem.Application.Validator.Reservation
{
    public class CreateReservationValidator : AbstractValidator<ReservationRequestDto>
    {
        public CreateReservationValidator() 
        {
            RuleFor(x => x.RoomId)
                .NotEmpty()
                .WithMessage("Room is required.");

            RuleFor(x => x.CheckIn)
                .GreaterThan(DateTime.Today)
                .WithMessage("Check-in date must be greater than today.");

            RuleFor(x => x.CheckOut)
                .GreaterThan(x => x.CheckIn)
                .WithMessage("Check-out date must be greater than check-in date.");

            RuleFor(x => x.Guests)
                .NotEmpty()
                .WithMessage("At least one guest is required.");

            RuleForEach(x => x.Guests)
                .SetValidator(new GuestValidator());

            RuleFor(x => x.EmergencyContact)
                .NotNull()
                .SetValidator(new EmergencyContactValidator());
        }
    }
}
