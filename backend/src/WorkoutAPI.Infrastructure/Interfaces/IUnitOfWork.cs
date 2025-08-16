namespace WorkoutAPI.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IUserCredentialsRepository UserCredentials { get; }
    ITrainerRepository Trainers { get; }
    IMemberRepository Members { get; }
    IPaymentRepository Payments { get; }
    IScheduleRepository Schedules { get; }
    IExerciseRepository Exercises { get; }
    IWorkoutSessionRepository WorkoutSessions { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

