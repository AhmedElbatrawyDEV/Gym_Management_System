using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutPlanExercise : BaseEntity
{
    public Guid WorkoutPlanId { get; set; }
    public Guid ExerciseId { get; set; }
    public int Order { get; set; }
    public int DefaultSets { get; set; }
    public int DefaultReps { get; set; }
    public TimeSpan DefaultRestTime { get; set; }
    public string? Notes { get; set; }
    
    // Navigation Properties
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;
    public virtual Exercise Exercise { get; set; } = null!;
}

