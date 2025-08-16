namespace WorkoutAPI.Application.DTOs;
public record PaymentRequest(Guid SubscriptionId, decimal Amount, string Method);
public record PaymentResult(bool Success, string TransactionId, string Message);