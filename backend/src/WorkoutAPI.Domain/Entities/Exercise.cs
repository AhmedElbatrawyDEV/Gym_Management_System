using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class Exercise : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public ExerciseType Type { get; set; }
    public MuscleGroup PrimaryMuscleGroup { get; set; }
    public MuscleGroup? SecondaryMuscleGroup { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public string? IconName { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public virtual ICollection<ExerciseTranslation> Translations { get; set; } = new List<ExerciseTranslation>();
    public virtual ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
    public virtual ICollection<WorkoutSessionExercise> WorkoutExerciseSessions { get; set; } = new List<WorkoutSessionExercise>();
}

