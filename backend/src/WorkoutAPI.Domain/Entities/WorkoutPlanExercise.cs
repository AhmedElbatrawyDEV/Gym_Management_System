
// Entities
using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutPlanExercise : Entity<WorkoutPlanExercise, Guid>
{
    public Guid WorkoutPlanId { get; private set; }
    public Guid ExerciseId { get; private set; }
    public int Day { get; private set; }
    public int Order { get; private set; }
    public int Sets { get; private set; }
    public int? Reps { get; private set; }
    public decimal? Weight { get; private set; }
    public TimeSpan? Duration { get; private set; }
    public TimeSpan? RestTime { get; private set; }
    public string? Notes { get; private set; }

    // Navigation properties
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;
    public virtual Exercise Exercise { get; set; } = null!;

    private WorkoutPlanExercise() { } // EF Core

    public static WorkoutPlanExercise CreateNew(Guid workoutPlanId, Guid exerciseId, int day, int order, int sets,
                                              int? reps = null, decimal? weight = null, TimeSpan? duration = null,
                                              TimeSpan? restTime = null, string? notes = null)
    {
        return new WorkoutPlanExercise
        {
            Id = Guid.NewGuid(),
            WorkoutPlanId = workoutPlanId,
            ExerciseId = exerciseId,
            Day = day > 0 ? day : throw new ArgumentException("Day must be positive"),
            Order = order > 0 ? order : throw new ArgumentException("Order must be positive"),
            Sets = sets > 0 ? sets : throw new ArgumentException("Sets must be positive"),
            Reps = reps,
            Weight = weight,
            Duration = duration,
            RestTime = restTime,
            Notes = notes
        };
    }

    public void UpdateSets(int sets, int? reps = null, decimal? weight = null, TimeSpan? duration = null)
    {
        Sets = sets > 0 ? sets : throw new ArgumentException("Sets must be positive");
        Reps = reps;
        Weight = weight;
        Duration = duration;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
    }
}
