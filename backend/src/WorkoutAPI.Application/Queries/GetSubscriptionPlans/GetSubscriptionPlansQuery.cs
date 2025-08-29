using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Queries.GetSubscriptionPlans;
public class GetSubscriptionPlansQuery : BaseQuery<List<SubscriptionPlanDto>>
{
    public bool? ActiveOnly { get; init; } = true;
}