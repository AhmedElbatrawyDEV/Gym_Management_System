using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public class ClassBookingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid ClassScheduleId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public DateTime ClassStartTime { get; set; }
    public DateTime ClassEndTime { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
}