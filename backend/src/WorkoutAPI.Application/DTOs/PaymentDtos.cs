using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public record CreatePaymentRequest(
    Guid MemberId,
    decimal Amount,
    DateTime PaymentDate,
    PaymentStatus Status,
    string Description
);

public record UpdatePaymentRequest(
    decimal Amount,
    DateTime PaymentDate,
    PaymentStatus Status,
    string Description
);

public record PaymentResponse(
    Guid Id,
    Guid MemberId,
    string MemberFullName,
    decimal Amount,
    DateTime PaymentDate,
    PaymentStatus Status,
    string Description,
    DateTime CreatedAt
);



namespace WorkoutAPI.Application.DTOs;
public record PaymentRequest(Guid SubscriptionId, decimal Amount, string Method);
public record PaymentResult(bool Success, string TransactionId, string Message);