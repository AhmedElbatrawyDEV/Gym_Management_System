namespace WorkoutAPI.Domain.ValueObjects;

public record ExerciseSetRecord(
    int SetNumber,
    int ActualReps,
    decimal ActualWeight,
    TimeSpan ActualRestTime,
    DateTime CompletedAt,
    string? Notes = null
);

