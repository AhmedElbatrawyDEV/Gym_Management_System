using FluentValidation;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Commands.CheckIn;

public class CheckInCommand : BaseCommand<Guid>
{
    public Guid UserId { get; init; }
    public ActivityType ActivityType { get; init; }
}
public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
{
    public CheckInCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.ActivityType)
            .IsInEnum().WithMessage("Invalid activity type");
    }
}
