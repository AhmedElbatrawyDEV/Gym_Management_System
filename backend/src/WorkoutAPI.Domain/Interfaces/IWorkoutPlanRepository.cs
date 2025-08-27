using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Interfaces;

public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
{
    Task<IEnumerable<WorkoutPlan>> GetByTypeAsync(WorkoutPlanType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkoutPlan>> GetByDifficultyAsync(DifficultyLevel difficulty, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkoutPlan>> GetActivePlansAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkoutPlan>> GetByCreatorAsync(Guid creatorId, CancellationToken cancellationToken = default);
}
