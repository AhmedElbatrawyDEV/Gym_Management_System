using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class Subscription : BaseEntity {
    public Guid MemberProfileId { get; set; }
    public Guid SubscriptionPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
}