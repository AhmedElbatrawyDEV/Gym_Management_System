using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Queries.GetWorkoutPlans;
public class GetWorkoutPlansQuery : BaseQuery<List<WorkoutPlanDto>>
{
    public Guid? CreatedBy { get; init; }
    public WorkoutPlanType? Type { get; init; }
    public DifficultyLevel? Difficulty { get; init; }
    public bool? ActiveOnly { get; init; } = true;
}
