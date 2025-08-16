using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class WorkoutExercise : BaseEntity
{
    public Guid WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; } = default!;
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = default!;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public double? WeightKg { get; set; }
}