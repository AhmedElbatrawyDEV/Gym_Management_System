using WorkoutAPI.Domain.Aggregates;

namespace WorkoutAPI.Domain.Interfaces;

public interface IWorkoutSessionRepository : IRepository<WorkoutSession> {
    Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkoutSession>> GetByTrainerIdAsync(Guid trainerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkoutSession>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkoutSession>> GetActiveSessionsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkoutSession>> GetScheduledSessionsAsync(DateTime date, CancellationToken cancellationToken = default);
}

