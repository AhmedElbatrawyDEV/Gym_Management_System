using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class UserCredentialsRepository : Repository<UserCredentials>, IUserCredentialsRepository
{
    public UserCredentialsRepository(WorkoutDbContext context) : base(context)
    {
    }

    public async Task<UserCredentials?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(uc => uc.User)
            .FirstOrDefaultAsync(uc => uc.UserId == userId);
    }

    public async Task<UserCredentials?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(uc => uc.User)
            .FirstOrDefaultAsync(uc => uc.User.Email == email);
    }

    public async Task<IEnumerable<UserCredentials>> GetByRoleAsync(UserRole role)
    {
        return await _dbSet
            .Include(uc => uc.User)
            .Where(uc => uc.Role == role)
            .OrderBy(uc => uc.User.FirstName)
            .ThenBy(uc => uc.User.LastName)
            .ToListAsync();
    }

    public async Task<bool> UpdateLastLoginAsync(Guid userId)
    {
        var credentials = await _dbSet.FirstOrDefaultAsync(uc => uc.UserId == userId);
        if (credentials == null) return false;

        credentials.LastLoginDate = DateTime.UtcNow;
        credentials.SetUpdated();
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> IncrementFailedLoginAttemptsAsync(Guid userId)
    {
        var credentials = await _dbSet.FirstOrDefaultAsync(uc => uc.UserId == userId);
        if (credentials == null) return false;

        credentials.FailedLoginAttempts++;
        credentials.SetUpdated();
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ResetFailedLoginAttemptsAsync(Guid userId)
    {
        var credentials = await _dbSet.FirstOrDefaultAsync(uc => uc.UserId == userId);
        if (credentials == null) return false;

        credentials.FailedLoginAttempts = 0;
        credentials.SetUpdated();
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> LockUserAsync(Guid userId)
    {
        var credentials = await _dbSet.FirstOrDefaultAsync(uc => uc.UserId == userId);
        if (credentials == null) return false;

        credentials.IsLocked = true;
        credentials.SetUpdated();
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UnlockUserAsync(Guid userId)
    {
        var credentials = await _dbSet.FirstOrDefaultAsync(uc => uc.UserId == userId);
        if (credentials == null) return false;

        credentials.IsLocked = false;
        credentials.FailedLoginAttempts = 0;
        credentials.SetUpdated();
        return await _context.SaveChangesAsync() > 0;
    }
}

