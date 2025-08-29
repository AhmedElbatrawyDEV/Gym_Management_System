using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetUserSubscriptions;

public class GetUserSubscriptionsQueryHandler : IRequestHandler<GetUserSubscriptionsQuery, Result<List<UserSubscriptionDto>>>
{
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public GetUserSubscriptionsQueryHandler(
        IUserSubscriptionRepository userSubscriptionRepository,
        IUserRepository userRepository,
        ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _userSubscriptionRepository = userSubscriptionRepository;
        _userRepository = userRepository;
        _subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public async Task<Result<List<UserSubscriptionDto>>> Handle(GetUserSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.UserSubscription> subscriptions;

        if (request.ActiveOnly == true)
        {
            subscriptions = await _userSubscriptionRepository.GetActiveByUserIdAsync(request.UserId, cancellationToken);
        }
        else
        {
            subscriptions = await _userSubscriptionRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        var userName = user?.PersonalInfo.FullName ?? string.Empty;

        var planIds = subscriptions.Select(s => s.SubscriptionPlanId).Distinct().ToList();
        var plans = await _subscriptionPlanRepository.GetByIdsAsync(planIds, cancellationToken);
        var planLookup = plans.ToDictionary(p => p.Id, p => p.Name);

        var result = subscriptions.Select(subscription => new UserSubscriptionDto
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            UserName = userName,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            SubscriptionPlanName = planLookup.GetValueOrDefault(subscription.SubscriptionPlanId, string.Empty),
            StartDate = subscription.Period.StartDate,
            EndDate = subscription.Period.EndDate,
            Status = subscription.Status,
            CreatedAt = subscription.CreatedAt,
            IsActive = subscription.IsActive,
            DaysRemaining = (int)(subscription.Period.EndDate - DateTime.UtcNow).TotalDays
        }).ToList();

        return Result<List<UserSubscriptionDto>>.Success(result);
    }
}
