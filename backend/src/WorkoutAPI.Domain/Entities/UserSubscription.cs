using WorkoutAPI.Domain.Common;


namespace WorkoutAPI.Domain.Entities {
    public class UserSubscription : BaseEntity {
        public Guid UserId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public bool AutoRenew { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public bool IsActive => Status == SubscriptionStatus.Active && EndDate > DateTime.UtcNow;
        public bool IsExpired => EndDate <= DateTime.UtcNow;
        public int RemainingDays => (EndDate - DateTime.UtcNow).Days;
    }
}