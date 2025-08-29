using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetUserPayments;

public class GetUserPaymentsQueryHandler : IRequestHandler<GetUserPaymentsQuery, Result<List<PaymentDto>>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetUserPaymentsQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Result<List<PaymentDto>>> Handle(GetUserPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var result = payments.Select(payment => new PaymentDto
        {
            Id = payment.Id,
            UserId = payment.UserId,
            UserSubscriptionId = payment.UserSubscriptionId,
            Amount = payment.Amount.Amount,
            Currency = payment.Amount.Currency,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            PaymentDate = payment.PaymentDate,
            TransactionId = payment.TransactionId,
            Description = payment.Description
        }).ToList();

        return Result<List<PaymentDto>>.Success(result);
    }
}
