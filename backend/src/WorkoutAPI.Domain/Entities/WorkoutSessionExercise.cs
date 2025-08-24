
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutSessionExercise : Entity<WorkoutSessionExercise, Guid> {
    private readonly List<ExerciseSetRecord> _sets = new();

    public Guid WorkoutSessionId { get; private set; }
    public Guid ExerciseId { get; private set; }
    public int Order { get; private set; }
    public bool IsCompleted { get; private set; } = false;
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public string? Notes { get; set; }

    public IReadOnlyCollection<ExerciseSetRecord> Sets => _sets.AsReadOnly();

    private WorkoutSessionExercise() { } // EF Core

    public static WorkoutSessionExercise CreateNew(Guid workoutSessionId, Guid exerciseId, int order) {
        return new WorkoutSessionExercise {
            Id = Guid.NewGuid(),
            WorkoutSessionId = workoutSessionId,
            ExerciseId = exerciseId,
            Order = order
        };
    }

    public void StartExercise() {
        if (IsCompleted)
            throw new InvalidOperationException("Exercise already completed");
        StartTime = DateTime.UtcNow;
    }

    public void CompleteSets(List<ExerciseSetRecord> sets) {
        if (IsCompleted)
            throw new InvalidOperationException("Exercise already completed");

        _sets.Clear();
        _sets.AddRange(sets ?? throw new ArgumentNullException(nameof(sets)));

        IsCompleted = true;
        EndTime = DateTime.UtcNow;
    }

    public void AddSet(ExerciseSetRecord set) {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot add sets to completed exercise");
        _sets.Add(set ?? throw new ArgumentNullException(nameof(set)));
    }
}
