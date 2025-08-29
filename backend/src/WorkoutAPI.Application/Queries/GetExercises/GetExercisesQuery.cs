using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Queries.GetExercises;

public class GetExercisesQuery : BasePaginatedQuery<ExerciseDto>
{
    public ExerciseType? Type { get; init; }
    public MuscleGroup? MuscleGroup { get; init; }
    public DifficultyLevel? Difficulty { get; init; }
    public bool? ActiveOnly { get; init; } = true;
    public Language Language { get; init; } = Language.English;
}