using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public class ExerciseDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ExerciseType Type { get; set; }
    public MuscleGroup PrimaryMuscleGroup { get; set; }
    public MuscleGroup? SecondaryMuscleGroup { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public string? IconName { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public string? Instructions { get; set; }
}
