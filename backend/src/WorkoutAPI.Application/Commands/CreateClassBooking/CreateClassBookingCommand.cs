using FluentValidation;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.CreateClassBooking;
public class CreateClassBookingCommand : BaseCommand<Guid>
{
    public Guid UserId { get; init; }
    public Guid ClassScheduleId { get; init; }
}
public class CreateClassBookingCommandValidator : AbstractValidator<CreateClassBookingCommand>
{
    public CreateClassBookingCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.ClassScheduleId)
            .NotEmpty().WithMessage("Class schedule ID is required");
    }
}
