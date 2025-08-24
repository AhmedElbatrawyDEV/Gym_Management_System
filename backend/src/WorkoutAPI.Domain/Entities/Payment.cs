using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

public class Payment : BaseEntity {
    public Guid UserId { get; set; }
    public Guid? UserSubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "SAR";
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? TransactionId { get; set; }
    public string? Description { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public UserSubscription? UserSubscription { get; set; }
    public Invoice? Invoice { get; set; }
}