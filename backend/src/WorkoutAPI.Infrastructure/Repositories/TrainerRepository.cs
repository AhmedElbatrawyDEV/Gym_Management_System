using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class TrainerRepository : BaseRepository<Trainer>, ITrainerRepository {
    public TrainerRepository(WorkoutDbContext context) : base(context) { }

    public async Task<IEnumerable<Trainer>> GetAvailableTrainersAsync(CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(t => t.IsAvailable)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Trainer>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(t => t.Specialization.ToLower().Contains(specialization.ToLower()) && t.IsAvailable)
            .ToListAsync(cancellationToken);
    }

    public async Task<Trainer?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) {
        return await _dbSet
            .FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken);
    }
}
