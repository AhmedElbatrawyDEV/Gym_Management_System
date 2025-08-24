
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums.WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Events;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Domain.Aggregates;

// PAYMENT AGGREGATE ROOT
public class Payment : AggregateRoot<Payment> {
    public Guid UserId { get; private set; }
    public Guid? UserSubscriptionId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaymentDate { get; private set; }
    public string? TransactionId { get; private set; }
    public string? Description { get; private set; }
    public Dictionary<string, string>? Metadata { get; private set; }

    public static Payment CreateNew(Guid userId, Money amount, PaymentMethod paymentMethod,
                                  string? description = null, Guid? userSubscriptionId = null) {
        var payment = BaseFactory.Create();
        payment.UserId = userId;
        payment.UserSubscriptionId = userSubscriptionId;
        payment.Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        payment.PaymentMethod = paymentMethod;
        payment.Status = PaymentStatus.Pending;
        payment.Description = description;
        payment.Metadata = new Dictionary<string, string>();
        return payment;
    }

    public void ProcessPayment(string transactionId) {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Can only process pending payments");

        Status = PaymentStatus.Completed;
        PaymentDate = DateTime.UtcNow;
        TransactionId = transactionId;

        AddEvent(new PaymentProcessedEvent(Guid, UserId, Amount.Amount));
    }

    public void FailPayment(string reason) {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Can only fail pending payments");

        Status = PaymentStatus.Failed;
        AddMetadata("failure_reason", reason);
    }

    public void RefundPayment(string reason) {
        if (Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Can only refund completed payments");

        Status = PaymentStatus.Refunded;
        AddMetadata("refund_reason", reason);
        AddMetadata("refund_date", DateTime.UtcNow.ToString("O"));
    }

    public void AddMetadata(string key, string value) {
        Metadata ??= new Dictionary<string, string>();
        Metadata[key] = value;
    }
}
