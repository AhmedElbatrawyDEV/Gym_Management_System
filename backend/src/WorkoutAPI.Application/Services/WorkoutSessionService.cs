using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Domain.ValueObjects;
public interface IWorkoutSessionService {
    Task<WorkoutSession> StartSessionAsync(Guid userId, Guid planId);
    Task CompleteExerciseAsync(Guid sessionId, Guid exerciseId, List<ExerciseSetRecord> sets);
    Task<IEnumerable<WorkoutSession>> GetUserSessionsAsync(Guid userId, DateTime? fromDate = null);

}
public class WorkoutSessionService : IWorkoutSessionService {
    private readonly IRepository<WorkoutSession> _sessionRepository;
    private readonly IRepository<WorkoutPlan> _planRepository;

    public WorkoutSessionService(
        IRepository<WorkoutSession> sessionRepository,
        IRepository<WorkoutPlan> planRepository) {
        _sessionRepository = sessionRepository;
        _planRepository = planRepository;
    }

    public async Task<WorkoutSession> StartSessionAsync(Guid userId, Guid planId) {
        var plan = await _planRepository.GetByIdAsync(planId);
        if (plan == null)
            throw new KeyNotFoundException("Workout plan not found");

        var session = new WorkoutSession(userId, planId, plan.TotalExercises);
        await _sessionRepository.AddAsync(session);
        return session;
    }

    public async Task CompleteExerciseAsync(Guid sessionId, Guid exerciseId, List<ExerciseSetRecord> sets) {
        var session = await _sessionRepository.GetByIdAsync(sessionId);
        if (session == null)
            throw new KeyNotFoundException("Session not found");

        session.CompleteExercise(exerciseId, sets);
        await _sessionRepository.UpdateAsync(session);
    }

    public async Task<IEnumerable<WorkoutSession>> GetUserSessionsAsync(Guid userId, DateTime? fromDate = null) {
        var query = _sessionRepository.Query()
            .Where(s => s.UserId == userId)
            .Include(s => s.Exercises)
            .ThenInclude(e => e.Sets)
            .OrderByDescending(s => s.StartTime);

        if (fromDate.HasValue)
            query = query.Where(s => s.StartTime >= fromDate.Value);

        return await query.ToListAsync();
    }

}