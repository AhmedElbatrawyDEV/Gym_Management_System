using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
{
    Task<IEnumerable<SubscriptionPlan>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionPlan>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionPlan>> GetByDurationAsync(int minDays, int maxDays, CancellationToken cancellationToken = default);
}

