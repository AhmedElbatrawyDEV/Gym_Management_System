using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.CancelUserSubscription;
public class CancelUserSubscriptionCommand : BaseCommand
{
    public Guid Id { get; init; }
}
