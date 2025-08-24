using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class SubscriptionPlanRepository : BaseRepository<SubscriptionPlan>, ISubscriptionPlanRepository {
    public SubscriptionPlanRepository(WorkoutDbContext context) : base(context) { }

    public async Task<IEnumerable<SubscriptionPlan>> GetActiveAsync(CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(sp => sp.IsActive)
            .OrderBy(sp => sp.Price.Amount)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(sp => sp.IsActive && sp.Price.Amount >= minPrice && sp.Price.Amount <= maxPrice)
            .OrderBy(sp => sp.Price.Amount)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetByDurationAsync(int minDays, int maxDays, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(sp => sp.IsActive && sp.DurationDays >= minDays && sp.DurationDays <= maxDays)
            .OrderBy(sp => sp.DurationDays)
            .ToListAsync(cancellationToken);
    }
}

