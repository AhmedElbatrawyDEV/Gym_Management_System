namespace WorkoutAPI.Application.DTOs;
public record UserWorkoutPlanResponse(Guid PlanId, string Title, string? Description, int ExerciseCount);
public record WorkoutSessionSummaryResponse(Guid PlanId, int TotalExercises, int TotalSets, int TotalReps);