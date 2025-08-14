using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public record StartWorkoutSessionRequest(
    Guid UserId,
    Guid WorkoutPlanId
);

public record CompleteExerciseRequest(
    Guid ExerciseId,
    List<ExerciseSetRequest> Sets
);

public record ExerciseSetRequest(
    int SetNumber,
    int Reps,
    decimal Weight,
    TimeSpan RestTime,
    string? Notes
);

public record CompleteWorkoutSessionRequest(
    string? Notes
);

public record WorkoutSessionResponse(
    Guid Id,
    Guid UserId,
    string UserName,
    Guid WorkoutPlanId,
    string WorkoutPlanName,
    ExerciseType WorkoutType,
    DateTime StartTime,
    DateTime? EndTime,
    WorkoutSessionStatus Status,
    int CompletedExercises,
    int TotalExercises,
    string? Notes,
    List<WorkoutExerciseSessionResponse> Exercises
);

public record WorkoutExerciseSessionResponse(
    Guid Id,
    Guid ExerciseId,
    string ExerciseName,
    string? ExerciseDescription,
    string? ExerciseInstructions,
    string? ExerciseIcon,
    int Order,
    bool IsCompleted,
    DateTime? StartTime,
    DateTime? EndTime,
    string? Notes,
    List<ExerciseSetRecordResponse> Sets
);

public record ExerciseSetRecordResponse(
    Guid Id,
    int SetNumber,
    int ActualReps,
    decimal ActualWeight,
    TimeSpan ActualRestTime,
    DateTime CompletedAt,
    string? Notes
);

public record WorkoutHistoryResponse(
    List<WorkoutSessionSummaryResponse> Sessions,
    int TotalSessions,
    int CompletedSessions,
    TimeSpan TotalWorkoutTime,
    decimal TotalWeightLifted
);

