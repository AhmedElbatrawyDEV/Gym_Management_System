using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Queries.GetUserPayments;
public class GetUserPaymentsQuery : BaseQuery<List<PaymentDto>>
{
    public Guid UserId { get; init; }
}