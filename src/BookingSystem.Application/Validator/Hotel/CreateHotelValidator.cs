using BookingSystem.Application.DTOs.Hotel;
using FluentValidation;

namespace BookingSystem.Application.Validator.Hotel
{
    public class CreateHotelValidator : AbstractValidator<HotelRequestDto>
    {
        public CreateHotelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
