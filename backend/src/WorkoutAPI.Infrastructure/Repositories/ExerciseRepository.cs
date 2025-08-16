using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(WorkoutDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Exercise>> GetByTypeAsync(ExerciseType type)
    {
        return await _dbSet
            .Where(e => e.Type == type && e.IsActive)
            .Include(e => e.Translations)
            .OrderBy(e => e.Code)
            .ToListAsync();
    }

    public async Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(MuscleGroup muscleGroup)
    {
        return await _dbSet
            .Where(e => (e.PrimaryMuscleGroup == muscleGroup || e.SecondaryMuscleGroup == muscleGroup) && e.IsActive)
            .Include(e => e.Translations)
            .OrderBy(e => e.Code)
            .ToListAsync();
    }

    public async Task<Exercise?> GetWithTranslationsAsync(Guid exerciseId)
    {
        return await _dbSet
            .Include(e => e.Translations)
            .FirstOrDefaultAsync(e => e.Id == exerciseId);
    }

    public async Task<IEnumerable<Exercise>> GetExercisesWithTranslationsAsync(Language language)
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .Include(e => e.Translations.Where(t => t.Language == language))
            .OrderBy(e => e.Type)
            .ThenBy(e => e.Code)
            .ToListAsync();
    }

    public async Task<Exercise?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .Include(e => e.Translations)
            .FirstOrDefaultAsync(e => e.Code == code);
    }
}

