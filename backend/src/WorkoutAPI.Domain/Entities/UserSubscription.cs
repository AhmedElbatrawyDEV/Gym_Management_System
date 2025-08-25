
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Domain.Entities;

// ENTITIES (Non-Aggregates)
public class UserSubscription : Entity<UserSubscription, Guid>
{
    public Guid UserId { get; private set; }
    public Guid SubscriptionPlanId { get; private set; }
    public DateRange Period { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static UserSubscription CreateNew(Guid userId, Guid subscriptionPlanId, DateRange period)
    {
        return new UserSubscription
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            SubscriptionPlanId = subscriptionPlanId,
            Period = period ?? throw new ArgumentNullException(nameof(period)),
            Status = SubscriptionStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
    }

    public bool IsActive => Status == SubscriptionStatus.Active && Period.IsActive;

    public void Cancel()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException("Can only cancel active subscriptions");
        Status = SubscriptionStatus.Cancelled;
    }

    public void Suspend()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException("Can only suspend active subscriptions");
        Status = SubscriptionStatus.Suspended;
    }

    public void Reactivate()
    {
        if (Status != SubscriptionStatus.Suspended)
            throw new InvalidOperationException("Can only reactivate suspended subscriptions");
        Status = SubscriptionStatus.Active;
    }
}
