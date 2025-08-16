using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;
namespace WorkoutAPI.Domain.Entities;
public class Payment : BaseEntity
{
    public Guid SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public PaymentStatus Status { get; set; } = PaymentStatus.Paid;
    public string? Provider { get; set; }
    public string? Reference { get; set; }
}