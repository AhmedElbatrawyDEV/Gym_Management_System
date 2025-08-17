using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;


namespace WorkoutAPI.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PaymentId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Payment Payment { get; set; } = null!;
    }
}