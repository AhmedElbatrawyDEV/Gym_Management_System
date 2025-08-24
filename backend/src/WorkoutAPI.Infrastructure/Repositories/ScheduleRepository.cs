using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class ScheduleRepository : Repository<Schedule>, IScheduleRepository {
    public ScheduleRepository(WorkoutDbContext context) : base(context) {
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesByTrainerIdAsync(Guid trainerId) {
        return await _dbSet
            .Include(s => s.Trainer)
                .ThenInclude(t => t.User)
            .Include(s => s.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .Where(s => s.TrainerId == trainerId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate) {
        return await _dbSet
            .Include(s => s.Trainer)
                .ThenInclude(t => t.User)
            .Include(s => s.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .Where(s => s.StartTime >= startDate && s.EndTime <= endDate)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Schedule>> GetAvailableSchedulesAsync() {
        return await _dbSet
            .Include(s => s.Trainer)
                .ThenInclude(t => t.User)
            .Include(s => s.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .Where(s => s.EnrolledCount < s.Capacity && s.StartTime > DateTime.UtcNow)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<Schedule?> GetScheduleWithDetailsAsync(Guid scheduleId) {
        return await _dbSet
            .Include(s => s.Trainer)
                .ThenInclude(t => t.User)
            .Include(s => s.WorkoutPlan)
                .ThenInclude(wp => wp.Translations)
            .FirstOrDefaultAsync(s => s.Id == scheduleId);
    }
}

