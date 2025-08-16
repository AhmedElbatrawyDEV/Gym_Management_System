using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class TrainerRepository : Repository<Trainer>, ITrainerRepository
{
    public TrainerRepository(WorkoutDbContext context) : base(context)
    {
    }

    public async Task<Trainer?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task<IEnumerable<Trainer>> GetAvailableTrainersAsync()
    {
        return await _dbSet
            .Include(t => t.User)
            .Where(t => t.IsAvailable && t.User.IsActive)
            .OrderBy(t => t.User.FirstName)
            .ThenBy(t => t.User.LastName)
            .ToListAsync();
    }

    public async Task<Trainer?> GetTrainerWithSchedulesAsync(Guid trainerId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.ScheduledSessions)
            .FirstOrDefaultAsync(t => t.Id == trainerId);
    }

    public async Task<IEnumerable<Trainer>> GetTrainersBySpecializationAsync(string specialization)
    {
        return await _dbSet
            .Include(t => t.User)
            .Where(t => t.Specialization.Contains(specialization) && t.IsAvailable)
            .OrderBy(t => t.User.FirstName)
            .ThenBy(t => t.User.LastName)
            .ToListAsync();
    }
}

