using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutPlan : BaseEntity {
    public string Code { get; set; } = string.Empty;
    public ExerciseType Type { get; set; }
    public ExerciseDifficulty Difficulty { get; set; }
    public int TotalExercises { get; set; }
    public int EstimatedDurationMinutes { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<WorkoutPlanTranslation> Translations { get; set; } = new List<WorkoutPlanTranslation>();
    public virtual ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
    public virtual ICollection<UserWorkoutPlan> UserWorkoutPlans { get; set; } = new List<UserWorkoutPlan>();
    public virtual ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
}

