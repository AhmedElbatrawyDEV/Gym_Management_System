
// Entities
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class Invoice : Entity<Invoice, Guid>
{
    public Guid UserId { get; private set; }
    public Guid PaymentId { get; private set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Payment Payment { get; set; } = null!;

    private Invoice() { } // EF Core

    public static Invoice CreateNew(Guid userId, Guid paymentId, decimal amount, decimal taxAmount)
    {
        var totalAmount = amount + taxAmount;
        var invoiceNumber = GenerateInvoiceNumber();

        return new Invoice
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PaymentId = paymentId,
            InvoiceNumber = invoiceNumber,
            Amount = amount,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            Status = InvoiceStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Send()
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Can only send draft invoices");

        Status = InvoiceStatus.Sent;
    }

    public void MarkAsViewed()
    {
        if (Status == InvoiceStatus.Sent)
            Status = InvoiceStatus.Viewed;
    }

    public void MarkAsPaid()
    {
        if (Status != InvoiceStatus.Sent && Status != InvoiceStatus.Viewed && Status != InvoiceStatus.Overdue)
            throw new InvalidOperationException("Invalid invoice status for payment");

        Status = InvoiceStatus.Paid;
        PaidAt = DateTime.UtcNow;
    }

    public void MarkAsOverdue()
    {
        if (Status == InvoiceStatus.Sent || Status == InvoiceStatus.Viewed)
            Status = InvoiceStatus.Overdue;
    }

    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Cannot cancel paid invoice");

        Status = InvoiceStatus.Cancelled;
    }

    public void Refund()
    {
        if (Status != InvoiceStatus.Paid)
            throw new InvalidOperationException("Can only refund paid invoices");

        Status = InvoiceStatus.Refunded;
    }

    public void PartialPayment()
    {
        if (Status != InvoiceStatus.Sent && Status != InvoiceStatus.Viewed && Status != InvoiceStatus.Overdue)
            throw new InvalidOperationException("Invalid invoice status for partial payment");

        Status = InvoiceStatus.PartiallyPaid;
    }

    private static string GenerateInvoiceNumber()
    {
        return $"INV{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
    }
}
