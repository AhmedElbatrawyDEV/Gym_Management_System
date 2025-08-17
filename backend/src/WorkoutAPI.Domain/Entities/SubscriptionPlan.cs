using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Entities.WorkoutAPI.Domain.Entities;
namespace WorkoutAPI.Domain.Entities;
public class SubscriptionPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public List<string> Features { get; set; } = new();
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}