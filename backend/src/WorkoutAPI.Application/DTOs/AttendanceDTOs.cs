using WorkoutAPI.Domain.Enums;

// DTOs/AttendanceDTOs.cs
namespace WorkoutAPI.Application.DTOs {
    public record CheckInRequest(
        Guid UserId,
        ActivityType ActivityType = ActivityType.GeneralWorkout
    );

    public record AttendanceRecordResponse(
        Guid Id,
        Guid UserId,
        DateTime CheckInTime,
        DateTime? CheckOutTime,
        int? DurationMinutes,
        ActivityType ActivityType
    );

    public record CreateGymClassRequest(
        string Name,
        string Description,
        Guid? InstructorId,
        int MaxCapacity,
        TimeSpan Duration,
        DifficultyLevel Difficulty = DifficultyLevel.Beginner
    );

    public record GymClassResponse(
        Guid Id,
        string Name,
        string Description,
        Guid? InstructorId,
        string? InstructorName,
        int MaxCapacity,
        int CurrentBookings,
        TimeSpan Duration,
        DifficultyLevel Difficulty,
        bool IsActive,
        DateTime CreatedAt
    );

    public record BookClassRequest(
        Guid UserId
    );

    public record ClassBookingResponse(
        Guid Id,
        Guid UserId,
        Guid ClassScheduleId,
        DateTime BookingDate,
        BookingStatus Status
    );
}