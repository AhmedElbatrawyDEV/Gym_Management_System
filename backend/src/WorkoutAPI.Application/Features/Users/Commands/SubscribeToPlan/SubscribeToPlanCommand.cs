using FluentValidation;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Features.Users.Commands.SubscribeToPlan
{
    public class SubscribeToPlanCommand : BaseCommand
    {
        public Guid UserId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
    }
    public class SubscribeToPlanCommandValidator : AbstractValidator<SubscribeToPlanCommand>
    {
        public SubscribeToPlanCommandValidator()
        {
            RuleFor(x => x).Custom((command, context) =>
            {
                if (command.UserId == Guid.Empty)
                    context.AddFailure($"UserId is requird and can't be {Guid.Empty}");

                if (command.SubscriptionPlanId == Guid.Empty)
                    context.AddFailure($"SubscriptionPlanId is requird and can't be {Guid.Empty}");
            });

        }

    }
}
