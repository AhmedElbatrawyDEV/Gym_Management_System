using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository {
    public UserRepository(WorkoutDbContext context) : base(context) {
    }

    public async Task<User?> GetByEmailAsync(string email) {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByPhoneAsync(string phoneNumber) {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync() {
        return await _dbSet
            .Where(u => u.IsActive)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync();
    }

    public async Task<User?> GetUserWithWorkoutPlansAsync(Guid userId) {
        return await _dbSet
            .Include(u => u.UserWorkoutPlans)
                .ThenInclude(uwp => uwp.WorkoutPlan)
                    .ThenInclude(wp => wp.Translations)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserWithSessionsAsync(Guid userId) {
        return await _dbSet
            .Include(u => u.WorkoutSessions)
                .ThenInclude(ws => ws.WorkoutPlan)
                    .ThenInclude(wp => wp.Translations)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}

