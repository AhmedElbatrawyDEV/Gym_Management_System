namespace WorkoutAPI.Domain.Interfaces;

public interface IUnitOfWork : IDisposable {
    IUserRepository Users { get; }
    IExerciseRepository Exercises { get; }
    IWorkoutSessionRepository WorkoutSessions { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

