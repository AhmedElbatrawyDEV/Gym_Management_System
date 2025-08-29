using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Queries.GetPaymentById;
public class GetUserPaymentsQuery : BasePaginatedQuery<PaymentDto>
{
    public Guid UserId { get; init; }
}