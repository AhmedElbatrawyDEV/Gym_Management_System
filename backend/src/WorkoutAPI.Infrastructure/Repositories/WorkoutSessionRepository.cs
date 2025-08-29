using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class WorkoutSessionRepository : BaseRepository<WorkoutSession>, IWorkoutSessionRepository
{
    public WorkoutSessionRepository(WorkoutDbContext context) : base(context) { }

    public async Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ws => ws.UserId == userId)
            .Include(ws => ws.Exercises)
            .OrderByDescending(ws => ws.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkoutSession>> GetByTrainerIdAsync(Guid trainerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ws => ws.TrainerId == trainerId)
            .Include(ws => ws.Exercises)
            .OrderByDescending(ws => ws.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkoutSession>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ws => ws.StartTime >= startDate && ws.StartTime <= endDate)
            .Include(ws => ws.Exercises)
            .OrderBy(ws => ws.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkoutSession>> GetActiveSessionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ws => ws.Status == WorkoutSessionStatus.InProgress)
            .Include(ws => ws.Exercises)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkoutSession>> GetScheduledSessionsAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

        return await _dbSet
            .Where(ws => ws.Status == WorkoutSessionStatus.Scheduled &&
                        ws.StartTime >= startOfDay && ws.StartTime <= endOfDay)
            .Include(ws => ws.Exercises)
            .OrderBy(ws => ws.StartTime)
            .ToListAsync(cancellationToken);
    }
}
