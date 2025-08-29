namespace WorkoutAPI.Application.DTOs;

public class WorkoutPlanExerciseDto
{
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public int Day { get; set; }
    public int Order { get; set; }
    public int Sets { get; set; }
    public int? Reps { get; set; }
    public decimal? Weight { get; set; }
    public TimeSpan? Duration { get; set; }
    public TimeSpan? RestTime { get; set; }
    public string? Notes { get; set; }
}