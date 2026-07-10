using BookingSystem.Application.DTOs.Room;
using BookingSystem.Domain.Enums;
using FluentValidation;

namespace BookingSystem.Application.Validator.Room
{
    public class UpdateRoomValidator : AbstractValidator<RoomRequestDto>
    {
        public UpdateRoomValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Type)
                 .IsInEnum()
                 .WithMessage($"The room type is invalid. Allowed values are: {string.Join(", ", Enum.GetNames<RoomType>())}.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0);

            RuleFor(x => x.BasePrice)
                .GreaterThan(0);

            RuleFor(x => x.Taxes)
                .GreaterThan(0);

            RuleFor(x => x.Location)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
