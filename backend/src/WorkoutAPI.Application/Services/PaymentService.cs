using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Services;

public interface IPaymentService {
    Task<PaymentResponse> ProcessPaymentAsync(ProcessPaymentRequest request);
    Task<PaymentResponse> GetPaymentAsync(Guid id);
    Task<IEnumerable<PaymentResponse>> GetUserPaymentsAsync(Guid userId);
    Task<InvoiceResponse> GetInvoiceAsync(Guid paymentId);
    Task<IEnumerable<InvoiceResponse>> GetUserInvoicesAsync(Guid userId);
}

public class PaymentService : IPaymentService {
    private readonly WorkoutDbContext _context;

    public PaymentService(WorkoutDbContext context) {
        _context = context;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(ProcessPaymentRequest request) {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        var payment = new Payment {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Amount = request.Amount,
            Currency = request.Currency,
            PaymentMethod = request.PaymentMethod,
            Status = PaymentStatus.Pending,
            Description = request.Description,
            Metadata = request.Metadata,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        // Simulate payment processing
        payment.Status = PaymentStatus.Completed;
        payment.PaymentDate = DateTime.UtcNow;
        payment.TransactionId = Guid.NewGuid().ToString("N")[..16];

        // Create invoice
        var invoice = new Invoice {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            PaymentId = payment.Id,
            InvoiceNumber = GenerateInvoiceNumber(),
            Amount = request.Amount,
            TaxAmount = request.Amount * 0.15m, // 15% VAT
            TotalAmount = request.Amount * 1.15m,
            Status = InvoiceStatus.Paid,
            PaidAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return MapPaymentToResponse(payment);
    }

    public async Task<PaymentResponse> GetPaymentAsync(Guid id) {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null)
            throw new KeyNotFoundException("Payment not found");

        return MapPaymentToResponse(payment);
    }

    public async Task<IEnumerable<PaymentResponse>> GetUserPaymentsAsync(Guid userId) {
        var payments = await _context.Payments
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return payments.Select(MapPaymentToResponse);
    }

    public async Task<InvoiceResponse> GetInvoiceAsync(Guid paymentId) {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.PaymentId == paymentId);

        if (invoice == null)
            throw new KeyNotFoundException("Invoice not found");

        return MapInvoiceToResponse(invoice);
    }

    public async Task<IEnumerable<InvoiceResponse>> GetUserInvoicesAsync(Guid userId) {
        var invoices = await _context.Invoices
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        return invoices.Select(MapInvoiceToResponse);
    }

    private static string GenerateInvoiceNumber() {
        return $"INV-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
    }

    private static PaymentResponse MapPaymentToResponse(Payment payment) {
        return new PaymentResponse(
            Id: payment.Id,
            UserId: payment.UserId,
            Amount: payment.Amount,
            Currency: payment.Currency,
            PaymentMethod: payment.PaymentMethod,
            Status: payment.Status,
            PaymentDate: payment.PaymentDate,
            TransactionId: payment.TransactionId,
            Description: payment.Description,
            CreatedAt: payment.CreatedAt
        );
    }

    private static InvoiceResponse MapInvoiceToResponse(Invoice invoice) {
        return new InvoiceResponse(
            Id: invoice.Id,
            InvoiceNumber: invoice.InvoiceNumber,
            UserId: invoice.UserId,
            PaymentId: invoice.PaymentId,
            Amount: invoice.Amount,
            TaxAmount: invoice.TaxAmount,
            TotalAmount: invoice.TotalAmount,
            Status: invoice.Status,
            CreatedAt: invoice.CreatedAt,
            PaidAt: invoice.PaidAt
        );
    }
}
