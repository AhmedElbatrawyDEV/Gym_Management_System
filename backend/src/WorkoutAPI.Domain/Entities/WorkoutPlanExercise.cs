using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutPlanExercise : BaseEntity {
    public Guid WorkoutPlanId { get; set; }
    public Guid ExerciseId { get; set; }
    public int Order { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; }
    public int RestSeconds { get; set; }
    public TimeSpan DefaultRestTime { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;
    public virtual Exercise Exercise { get; set; } = null!;
}

