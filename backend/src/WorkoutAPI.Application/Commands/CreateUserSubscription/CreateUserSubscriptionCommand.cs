using FluentValidation;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.CreateUserSubscription;

public class CreateUserSubscriptionCommand : BaseCommand<Guid>
{
    public Guid UserId { get; init; }
    public Guid SubscriptionPlanId { get; init; }
}

public class CreateUserSubscriptionCommandValidator : AbstractValidator<CreateUserSubscriptionCommand>
{
    public CreateUserSubscriptionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.SubscriptionPlanId)
            .NotEmpty().WithMessage("Subscription plan ID is required");
    }
}
