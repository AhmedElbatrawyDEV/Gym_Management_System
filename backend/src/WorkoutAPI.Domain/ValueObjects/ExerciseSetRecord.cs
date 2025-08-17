namespace WorkoutAPI.Domain.ValueObjects;

public record ExerciseSetRecord(
    int SetNumber,
    int Reps,
    decimal Weight,
    TimeSpan RestSeconds,
    DateTime CompletedAt,
    string? Notes = null
);

