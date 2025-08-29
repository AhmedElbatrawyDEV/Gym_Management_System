using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(WorkoutDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.ContactInfo.Email == email.ToLower(), cancellationToken);
    }

    public async Task<User?> GetByMembershipNumberAsync(string membershipNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.MembershipNumber == membershipNumber, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Status == UserStatus.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersByStatusAsync(UserStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(u => u.ContactInfo.Email == email.ToLower());

        if (excludeUserId.HasValue)
        {
            query = query.Where(u => u.Id != excludeUserId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync(cancellationToken);

        var term = searchTerm.ToLower();
        return await _dbSet
            .Where(u => u.PersonalInfo.FirstName.ToLower().Contains(term) ||
                       u.PersonalInfo.LastName.ToLower().Contains(term) ||
                       u.ContactInfo.Email.Contains(term) ||
                       u.MembershipNumber.Contains(term))
            .ToListAsync(cancellationToken);
    }
    public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedAsync(
       int pageNumber,
       int pageSize,
       string? searchTerm,
       UserStatus? status,
       CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(u =>
                u.PersonalInfo.FirstName.ToLower().Contains(term) ||
                u.PersonalInfo.LastName.ToLower().Contains(term) ||
                u.ContactInfo.Email.Contains(term) ||
                u.MembershipNumber.Contains(term));
        }

        if (status.HasValue)
        {
            query = query.Where(u => u.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (users, totalCount);
    }

    public async  Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        if (userIds == null || !userIds.Any())
            return Enumerable.Empty<User>();

        return await _dbSet
            .Where(e => userIds.Contains(e.Id) )
            .ToListAsync(cancellationToken);
    }
}
