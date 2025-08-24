using Microsoft.EntityFrameworkCore.Storage;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;
using WorkoutAPI.Infrastructure.Repositories;

namespace WorkoutAPI.Infrastructure;

public class UnitOfWork : IUnitOfWork {
    private readonly WorkoutDbContext _context;
    private IDbContextTransaction? _transaction;

    private IUserRepository? _users;
    private IExerciseRepository? _exercises;
    private IWorkoutSessionRepository? _workoutSessions;

    public UnitOfWork(WorkoutDbContext context) {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IExerciseRepository Exercises => _exercises ??= new ExerciseRepository(_context);
    public IWorkoutSessionRepository WorkoutSessions => _workoutSessions ??= new WorkoutSessionRepository(_context);

    public async Task<int> SaveChangesAsync() {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync() {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync() {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync() {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose() {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

