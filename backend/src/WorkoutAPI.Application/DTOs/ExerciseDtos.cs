namespace WorkoutAPI.Application.DTOs;
public record CreateExerciseRequest(string Name, string? Description, string? TargetMuscle, string? Equipment);
public record ExerciseResponse(Guid Id, string Name, string? Description, string? TargetMuscle, string? Equipment);