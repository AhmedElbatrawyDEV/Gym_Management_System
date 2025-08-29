using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Queries.GetUserSubscriptions;
public class GetUserSubscriptionsQuery : BaseQuery<List<UserSubscriptionDto>>
{
    public Guid UserId { get; init; }
    public bool? ActiveOnly { get; init; }
}