

using FluentValidation;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Features.Users.Commands.DeactivateUser;

public class DeactivateUserCommand : BaseCommand
{
    public Guid Id { get; init; }
}

public class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator()
    {
        RuleFor(x => x).Custom((command, context) =>
        {
            if (command.Id == Guid.Empty)
                context.AddFailure($"Id is requird and can't be {Guid.Empty}");
        });
    }
}


