using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class WorkoutSessionRepository : Repository<WorkoutSession>, IWorkoutSessionRepository {
    public WorkoutSessionRepository(WorkoutDbContext context) : base(context) {
    }

    public async Task<IEnumerable<WorkoutSession>> GetUserSessionsAsync(Guid userId) {
        return await _dbSet
            .Where(ws => ws.UserId == userId)
            .Include(ws => ws.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .OrderByDescending(ws => ws.StartTime)
            .ToListAsync();
    }

    public async Task<WorkoutSession?> GetActiveSessionAsync(Guid userId) {
        return await _dbSet
            .Where(ws => ws.UserId == userId &&
                        (ws.Status == WorkoutSessionStatus.InProgress || ws.Status == WorkoutSessionStatus.Paused))
            .Include(ws => ws.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .FirstOrDefaultAsync();
    }

    public async Task<WorkoutSession?> GetSessionWithExercisesAsync(Guid sessionId) {
        return await _dbSet
            .Where(ws => ws.Id == sessionId)
            .Include(ws => ws.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .Include(ws => ws.Exercises)
                .ThenInclude(wes => wes.Exercise)
                    .ThenInclude(e => e.Translations)
            .Include(ws => ws.Exercises)
                .ThenInclude(wes => wes.Sets)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<WorkoutSession>> GetCompletedSessionsAsync(Guid userId, DateTime? fromDate = null) {
        var query = _dbSet
            .Where(ws => ws.UserId == userId && ws.Status == WorkoutSessionStatus.Completed);

        if (fromDate.HasValue)
        {
            query = query.Where(ws => ws.StartTime >= fromDate.Value);
        }

        return await query
            .Include(ws => ws.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .OrderByDescending(ws => ws.StartTime)
            .ToListAsync();
    }

    public async Task<WorkoutSession?> GetLatestSessionAsync(Guid userId) {
        return await _dbSet
            .Where(ws => ws.UserId == userId)
            .Include(ws => ws.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .OrderByDescending(ws => ws.StartTime)
            .FirstOrDefaultAsync();
    }
}

