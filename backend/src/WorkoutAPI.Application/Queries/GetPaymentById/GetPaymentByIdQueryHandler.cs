using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetPaymentById;
public class GetUserPaymentsQueryHandler : IRequestHandler<GetUserPaymentsQuery, PaginatedResult<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetUserPaymentsQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaginatedResult<PaymentDto>> Handle(GetUserPaymentsQuery request, CancellationToken cancellationToken)
    {
        var (payments, totalCount) = await _paymentRepository.GetPaginatedByUserAsync(
            request.PageNumber,
            request.PageSize,
            request.UserId,
            cancellationToken);

        var paymentDtos = payments.Select(payment => new PaymentDto
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
        }).ToList().AsReadOnly();

        return PaginatedResult<PaymentDto>.Success(paymentDtos, request.PageNumber, request.PageSize, totalCount);
    }
}
