using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public class AttendanceRecordDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public int? DurationMinutes { get; set; }
    public ActivityType ActivityType { get; set; }
    public bool IsCheckedIn { get; set; }
}
