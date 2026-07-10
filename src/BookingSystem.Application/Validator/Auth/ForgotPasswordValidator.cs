using BookingSystem.Application.DTOs.Auth;
using FluentValidation;

namespace BookingSystem.Application.Validator.Auth
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
