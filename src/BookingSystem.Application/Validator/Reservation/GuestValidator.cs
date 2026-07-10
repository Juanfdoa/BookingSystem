using BookingSystem.Application.DTOs.Reservation;
using FluentValidation;

namespace BookingSystem.Application.Validator.Reservation
{
    public class GuestValidator : AbstractValidator<GuestRequestDto>
    {
        public GuestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage("Gender is invalid.");

            RuleFor(x => x.DocumentType)
                .IsInEnum()
                .WithMessage("Document type is invalid.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
