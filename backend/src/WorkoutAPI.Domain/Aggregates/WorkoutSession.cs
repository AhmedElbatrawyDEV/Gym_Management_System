
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Events;

namespace WorkoutAPI.Domain.Aggregates;

// WORKOUT SESSION AGGREGATE ROOT
public class WorkoutSession : AggregateRoot<WorkoutSession>
{
    private readonly List<WorkoutSessionExercise> _exercises = new();

    public Guid UserId { get; private set; }
    public Guid? TrainerId { get; private set; }
    public string Title { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public WorkoutSessionStatus Status { get; private set; }
    public string? Notes { get; private set; }

    public IReadOnlyCollection<WorkoutSessionExercise> Exercises => _exercises.AsReadOnly();

    public static WorkoutSession CreateNew(Guid userId, string title, DateTime startTime, Guid? trainerId = null)
    {
        var session = BaseFactory.Create();
        session.UserId = userId;
        session.TrainerId = trainerId;
        session.Title = title ?? throw new ArgumentNullException(nameof(title));
        session.StartTime = startTime;
        session.Status = WorkoutSessionStatus.Scheduled;
        return session;
    }

    public void StartSession()
    {
        if (Status != WorkoutSessionStatus.Scheduled)
            throw new InvalidOperationException("Can only start scheduled sessions");

        Status = WorkoutSessionStatus.InProgress;
        StartTime = DateTime.UtcNow;
    }

    public void CompleteSession(string? notes = null)
    {
        if (Status != WorkoutSessionStatus.InProgress)
            throw new InvalidOperationException("Can only complete sessions in progress");

        EndTime = DateTime.UtcNow;
        Status = WorkoutSessionStatus.Completed;
        Notes = notes;

        var duration = EndTime.Value - StartTime;
        AddEvent(new WorkoutSessionCompletedEvent(Id, UserId, duration));
    }

    public void CancelSession(string reason)
    {
        if (Status == WorkoutSessionStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed session");

        Status = WorkoutSessionStatus.Cancelled;
        Notes = $"Cancelled: {reason}";
    }

    public void AddExercise(Exercise exercise, int order)
    {
        if (Status != WorkoutSessionStatus.Scheduled)
            throw new InvalidOperationException("Can only add exercises to scheduled sessions");

        var sessionExercise = WorkoutSessionExercise.CreateNew(Id, exercise.Id, order);
        _exercises.Add(sessionExercise);
    }

    public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;
}
