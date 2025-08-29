using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class UserSubscriptionRepository : IUserSubscriptionRepository
{
    private readonly WorkoutDbContext _context;

    public UserSubscriptionRepository(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<UserSubscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserSubscriptions
            .Include(us => us.Period)
            .FirstOrDefaultAsync(us => us.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<UserSubscription>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.UserSubscriptions
            .Include(us => us.Period)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserSubscription> AddAsync(UserSubscription entity, CancellationToken cancellationToken = default)
    {
        await _context.UserSubscriptions.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<UserSubscription> UpdateAsync(UserSubscription entity, CancellationToken cancellationToken = default)
    {
        _context.UserSubscriptions.Update(entity);
        return await Task.FromResult(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.UserSubscriptions.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserSubscriptions.AnyAsync(us => us.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<UserSubscription>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserSubscriptions
            .Include(us => us.Period)
            .Where(us => us.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserSubscription>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserSubscriptions
            .Include(us => us.Period)
            .Where(us => us.UserId == userId && us.Status == Domain.Enums.SubscriptionStatus.Active && us.Period.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserSubscription?> GetActiveSubscriptionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserSubscriptions
            .Include(us => us.Period)
            .FirstOrDefaultAsync(us => us.UserId == userId && us.Status == Domain.Enums.SubscriptionStatus.Active && us.Period.IsActive, cancellationToken);
    }
}
