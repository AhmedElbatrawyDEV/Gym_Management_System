using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public class WorkoutPlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkoutPlanType Type { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int DurationWeeks { get; set; }
    public Guid CreatedBy { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<WorkoutPlanExerciseDto> Exercises { get; set; } = new();
}
