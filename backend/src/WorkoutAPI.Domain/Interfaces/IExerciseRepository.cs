using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Interfaces;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetByTypeAsync(ExerciseType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(MuscleGroup muscleGroup, CancellationToken cancellationToken = default);
    Task<IEnumerable<Exercise>> GetByDifficultyAsync(DifficultyLevel difficulty, CancellationToken cancellationToken = default);
    Task<Exercise?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Exercise>> GetActiveExercisesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Exercise>> SearchAsync(string searchTerm, Language language, CancellationToken cancellationToken = default);
}

