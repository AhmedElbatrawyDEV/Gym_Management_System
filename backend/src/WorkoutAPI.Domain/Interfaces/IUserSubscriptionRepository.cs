using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface IUserSubscriptionRepository : IRepository<UserSubscription>
{
    Task<IEnumerable<UserSubscription>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserSubscription>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserSubscription?> GetActiveSubscriptionAsync(Guid userId, CancellationToken cancellationToken = default);
}
