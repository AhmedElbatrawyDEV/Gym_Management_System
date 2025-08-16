namespace WorkoutAPI.Application.DTOs;

public record CreateScheduleRequest(
    string Title,
    string Description,
    DateTime StartTime,
    DateTime EndTime,
    Guid? TrainerId,
    Guid? WorkoutPlanId,
    int Capacity
);

public record UpdateScheduleRequest(
    string Title,
    string Description,
    DateTime StartTime,
    DateTime EndTime,
    Guid? TrainerId,
    Guid? WorkoutPlanId,
    int Capacity,
    int EnrolledCount
);

public record ScheduleResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime StartTime,
    DateTime EndTime,
    Guid? TrainerId,
    string? TrainerFullName,
    Guid? WorkoutPlanId,
    string? WorkoutPlanName,
    int Capacity,
    int EnrolledCount,
    DateTime CreatedAt
);


