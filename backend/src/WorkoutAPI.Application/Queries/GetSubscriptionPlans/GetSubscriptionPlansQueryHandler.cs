using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetSubscriptionPlans;

public class GetSubscriptionPlansQueryHandler : IRequestHandler<GetSubscriptionPlansQuery, Result<List<SubscriptionPlanDto>>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public GetSubscriptionPlansQueryHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public async Task<Result<List<SubscriptionPlanDto>>> Handle(GetSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.SubscriptionPlan> subscriptionPlans;

        if (request.ActiveOnly == true)
        {
            subscriptionPlans = await _subscriptionPlanRepository.GetActiveAsync(cancellationToken);
        }
        else
        {
            subscriptionPlans = await _subscriptionPlanRepository.GetAllAsync(cancellationToken);
        }

        var result = subscriptionPlans.Select(plan => new SubscriptionPlanDto
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            Price = plan.Price.Amount,
            Currency = plan.Price.Currency,
            DurationDays = plan.DurationDays,
            Features = plan.Features,
            IsActive = plan.IsActive
        }).ToList();

        return Result<List<SubscriptionPlanDto>>.Success(result);
    }
}
