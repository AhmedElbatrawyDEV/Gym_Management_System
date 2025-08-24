using System.Data;

namespace WorkoutAPI.Domain.Interfaces;

public interface IUnitOfWork {
    [Obsolete("Please use the BeginAsync version")]
    void Begin();

    [Obsolete("Please use the BeginAsync version")]
    void Begin(IsolationLevel isolationLevel);

    Task BeginAsync();

    Task BeginAsync(IsolationLevel isolationLevel);

    Task Commit();

    Task Rollback();
}
