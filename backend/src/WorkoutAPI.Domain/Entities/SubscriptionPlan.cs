using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class SubscriptionPlan : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
}