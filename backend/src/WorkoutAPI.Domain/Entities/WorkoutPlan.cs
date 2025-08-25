
// Entities
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutPlan : Entity<WorkoutPlan, Guid>
{
    private readonly List<WorkoutPlanExercise> _exercises = new();

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public WorkoutPlanType Type { get; private set; }
    public DifficultyLevel DifficultyLevel { get; private set; }
    public int DurationWeeks { get; private set; }
    public Guid CreatedBy { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<WorkoutPlanExercise> Exercises => _exercises.AsReadOnly();

    // Navigation properties
    public virtual User Creator { get; set; } = null!;
    public virtual ICollection<UserWorkoutPlan> UserWorkoutPlans { get; set; } = new List<UserWorkoutPlan>();

    private WorkoutPlan() { } // EF Core

    public static WorkoutPlan CreateNew(string name, WorkoutPlanType type, DifficultyLevel difficultyLevel,
                                      int durationWeeks, Guid createdBy, string? description = null)
    {
        return new WorkoutPlan
        {
            Id = Guid.NewGuid(),
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            Description = description,
            Type = type,
            DifficultyLevel = difficultyLevel,
            DurationWeeks = durationWeeks > 0 ? durationWeeks : throw new ArgumentException("Duration must be positive"),
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AddExercise(Exercise exercise, int day, int order, int sets,
                           int? reps = null, decimal? weight = null, TimeSpan? duration = null,
                           TimeSpan? restTime = null, string? notes = null)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot add exercises to inactive workout plan");

        if (_exercises.Any(e => e.Day == day && e.Order == order))
            throw new InvalidOperationException($"Exercise already exists at day {day}, order {order}");

        var planExercise = WorkoutPlanExercise.CreateNew(Id, exercise.Id, day, order, sets,
                                                       reps, weight, duration, restTime, notes);
        _exercises.Add(planExercise);
    }

    public void RemoveExercise(Guid exerciseId, int day)
    {
        var exercise = _exercises.FirstOrDefault(e => e.ExerciseId == exerciseId && e.Day == day);
        if (exercise != null)
        {
            _exercises.Remove(exercise);
        }
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void UpdateDetails(string name, string? description = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
    }
}
