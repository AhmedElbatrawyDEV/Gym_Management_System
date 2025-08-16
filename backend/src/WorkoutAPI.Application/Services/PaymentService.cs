using WorkoutAPI.Application.Abstractions;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Services;
public class PaymentService : IPaymentProcessor
{
    public Task<PaymentResult> CaptureAsync(PaymentRequest request)
    {
        // Mocked payment capture
        var tx = Guid.NewGuid().ToString("N").ToUpperInvariant();
        return Task.FromResult(new PaymentResult(true, tx, "Captured"));
    }
}