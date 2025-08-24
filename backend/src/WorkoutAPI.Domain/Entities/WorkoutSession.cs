using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Events;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutSession : AggregateRoot {
    private Guid _userId;
    private Guid _workoutPlanId;
    private DateTime _startTime;
    private DateTime? _endTime;
    private WorkoutSessionStatus _status = WorkoutSessionStatus.NotStarted;
    private int _completedExercises = 0;
    private int _totalExercises;
    private string? _notes;

    public Guid UserId => _userId;
    public Guid WorkoutPlanId => _workoutPlanId;
    public DateTime StartTime => _startTime;
    public DateTime? EndTime => _endTime;
    public WorkoutSessionStatus Status => _status;
    public int CompletedExercises => _completedExercises;
    public int TotalExercises => _totalExercises;
    public string? Notes => _notes;

    private readonly List<WorkoutSessionExercise> _exercises = new();
    public virtual IReadOnlyCollection<WorkoutSessionExercise> Exercises => _exercises.AsReadOnly();

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;

    // Constructors
    private WorkoutSession() { } // For EF Core

    public WorkoutSession(Guid userId, Guid workoutPlanId, int totalExercises) {
        _userId = userId;
        _workoutPlanId = workoutPlanId;
        _totalExercises = totalExercises;
        _status = WorkoutSessionStatus.NotStarted;
    }

    // Domain Methods
    public void StartSession() {
        if (_status != WorkoutSessionStatus.NotStarted)
            throw new InvalidOperationException("Session already started");

        _status = WorkoutSessionStatus.InProgress;
        _startTime = DateTime.UtcNow;

        AddDomainEvent(new WorkoutSessionStartedEvent(_userId, Id, _workoutPlanId));
    }

    public void CompleteExercise(Guid exerciseId, List<ExerciseSetRecord> sets) {
        var exercise = _exercises.FirstOrDefault(e => e.ExerciseId == exerciseId);
        if (exercise == null)
            throw new ArgumentException("Exercise not found in session");

        exercise.CompleteSets(sets);
        _completedExercises++;

        var totalWeight = sets.Sum(s => s.Weight * s.Reps);
        AddDomainEvent(new ExerciseCompletedEvent(_userId, Id, exerciseId, sets.Count, totalWeight));

        if (_completedExercises >= _totalExercises)
        {
            CompleteSession();
        }
    }

    public void CompleteSession(string? notes = null) {
        if (_status != WorkoutSessionStatus.InProgress)
            throw new InvalidOperationException("Session is not in progress");

        _status = WorkoutSessionStatus.Completed;
        _endTime = DateTime.UtcNow;
        _notes = notes;

        var duration = _endTime.Value - _startTime;
        AddDomainEvent(new WorkoutSessionCompletedEvent(_userId, Id, duration, _completedExercises));
    }

    public void PauseSession() {
        if (_status != WorkoutSessionStatus.InProgress)
            throw new InvalidOperationException("Session is not in progress");

        _status = WorkoutSessionStatus.Paused;
    }

    public void ResumeSession() {
        if (_status != WorkoutSessionStatus.Paused)
            throw new InvalidOperationException("Session is not paused");

        _status = WorkoutSessionStatus.InProgress;
    }

    public void CancelSession() {
        if (_status == WorkoutSessionStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed session");

        _status = WorkoutSessionStatus.Cancelled;
        _endTime = DateTime.UtcNow;
    }
}

