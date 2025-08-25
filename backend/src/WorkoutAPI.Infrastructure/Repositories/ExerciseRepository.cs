using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository {
    public ExerciseRepository(WorkoutDbContext context) : base(context) { }

    public async Task<IEnumerable<Exercise>> GetByTypeAsync(ExerciseType type, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(e => e.Type == type && e.IsActive)
            .Include(e => e.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(MuscleGroup muscleGroup, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(e => (e.PrimaryMuscleGroup == muscleGroup || e.SecondaryMuscleGroup == muscleGroup) && e.IsActive)
            .Include(e => e.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Exercise>> GetByDifficultyAsync(DifficultyLevel difficulty, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(e => e.Difficulty == difficulty && e.IsActive)
            .Include(e => e.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task<Exercise?> GetByCodeAsync(string code, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Include(e => e.Translations)
            .FirstOrDefaultAsync(e => e.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Exercise>> GetActiveExercisesAsync(CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(e => e.IsActive)
            .Include(e => e.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Exercise>> SearchAsync(string searchTerm, Language language, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetActiveExercisesAsync(cancellationToken);

        var term = searchTerm.ToLower();
        return await _dbSet
            .Where(e => e.IsActive)
            .Include(e => e.Translations)
            .Where(e => e.Code.ToLower().Contains(term) ||
                       e.Translations.Any(t => t.Language == language &&
                                             (t.Name.ToLower().Contains(term) ||
                                              t.Description.ToLower().Contains(term))))
            .ToListAsync(cancellationToken);
    }
}
