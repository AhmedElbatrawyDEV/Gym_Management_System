
// Entities
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums.WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

// ===== ADDITIONAL MISSING ENTITIES =====

public class AttendanceRecord : Entity<AttendanceRecord, Guid> {
    public Guid UserId { get; private set; }
    public DateTime CheckInTime { get; private set; }
    public DateTime? CheckOutTime { get; private set; }
    public int? DurationMinutes { get; private set; }
    public ActivityType ActivityType { get; private set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;

    private AttendanceRecord() { } // EF Core

    public static AttendanceRecord CreateNew(Guid userId, ActivityType activityType) {
        return new AttendanceRecord {
            Id = Guid.NewGuid(),
            UserId = userId,
            CheckInTime = DateTime.UtcNow,
            ActivityType = activityType
        };
    }

    public void CheckOut() {
        if (CheckOutTime.HasValue)
            throw new InvalidOperationException("User has already checked out");

        CheckOutTime = DateTime.UtcNow;
        DurationMinutes = (int)(CheckOutTime.Value - CheckInTime).TotalMinutes;
    }

    public bool IsCheckedIn => !CheckOutTime.HasValue;
    public TimeSpan? Duration => CheckOutTime?.Subtract(CheckInTime);
}
