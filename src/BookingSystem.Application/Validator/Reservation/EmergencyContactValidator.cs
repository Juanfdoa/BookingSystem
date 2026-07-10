using BookingSystem.Application.DTOs.Reservation;
using FluentValidation;

namespace BookingSystem.Application.Validator.Reservation
{
    public class EmergencyContactValidator : AbstractValidator<EmergencyContactRequestDto>
    {
        public EmergencyContactValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
