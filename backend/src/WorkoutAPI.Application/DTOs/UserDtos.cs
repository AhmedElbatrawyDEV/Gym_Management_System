using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    DateTime DateOfBirth,
    Gender Gender,
    string? ProfileImageUrl
);

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    DateTime DateOfBirth,
    Gender Gender,
    string? ProfileImageUrl
);

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? PhoneNumber,
    DateTime DateOfBirth,
    int Age,
    Gender Gender,
    string? ProfileImageUrl,
    bool IsActive,
    DateTime CreatedAt
);

public record UserProfileResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? PhoneNumber,
    DateTime DateOfBirth,
    int Age,
    Gender Gender,
    string? ProfileImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    List<UserWorkoutPlanResponse> WorkoutPlans,
    List<WorkoutSessionSummaryResponse> RecentSessions
);

public record UserWorkoutPlanResponse(
    Guid Id,
    string PlanName,
    ExerciseType Type,
    DateTime AssignedDate,
    DateTime? CompletedDate,
    bool IsActive,
    int CompletedSessions
);

public record WorkoutSessionSummaryResponse(
    Guid Id,
    string WorkoutPlanName,
    ExerciseType Type,
    DateTime StartTime,
    DateTime? EndTime,
    WorkoutSessionStatus Status,
    int CompletedExercises,
    int TotalExercises
);

