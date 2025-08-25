
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Domain.Entities;

public class SubscriptionPlan : Entity<SubscriptionPlan, Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; }
    public int DurationDays { get; private set; }
    public List<string> Features { get; private set; } = new();
    public bool IsActive { get; private set; } = true;

    private SubscriptionPlan() { } // EF Core

    public static SubscriptionPlan CreateNew(string name, string description, Money price,
                                           int durationDays, List<string> features)
    {
        return new SubscriptionPlan
        {
            Id = Guid.NewGuid(),
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            Description = description ?? throw new ArgumentNullException(nameof(description)),
            Price = price ?? throw new ArgumentNullException(nameof(price)),
            DurationDays = durationDays > 0 ? durationDays : throw new ArgumentException("Duration must be positive"),
            Features = features ?? new List<string>()
        };
    }

    public void UpdatePrice(Money newPrice)
    {
        Price = newPrice ?? throw new ArgumentNullException(nameof(newPrice));
    }

    public void AddFeature(string feature)
    {
        if (string.IsNullOrWhiteSpace(feature))
            throw new ArgumentException("Feature cannot be empty");

        if (!Features.Contains(feature))
            Features.Add(feature);
    }

    public void RemoveFeature(string feature)
    {
        Features.Remove(feature);
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
