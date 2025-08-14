using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Interfaces;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetByTypeAsync(ExerciseType type);
    Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(MuscleGroup muscleGroup);
    Task<Exercise?> GetWithTranslationsAsync(Guid exerciseId);
    Task<IEnumerable<Exercise>> GetExercisesWithTranslationsAsync(Language language);
    Task<Exercise?> GetByCodeAsync(string code);
}

