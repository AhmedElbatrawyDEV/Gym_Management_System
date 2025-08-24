using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface IWorkoutSessionRepository : IRepository<WorkoutSession> {
    Task<IEnumerable<WorkoutSession>> GetUserSessionsAsync(Guid userId);
    Task<WorkoutSession?> GetActiveSessionAsync(Guid userId);
    Task<WorkoutSession?> GetSessionWithExercisesAsync(Guid sessionId);
    Task<IEnumerable<WorkoutSession>> GetCompletedSessionsAsync(Guid userId, DateTime? fromDate = null);
    Task<WorkoutSession?> GetLatestSessionAsync(Guid userId);
}

