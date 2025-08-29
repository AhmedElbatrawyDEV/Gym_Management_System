using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public class WorkoutSessionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid? TrainerId { get; set; }
    public string? TrainerName { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan? Duration { get; set; }
    public WorkoutSessionStatus Status { get; set; }
    public string? Notes { get; set; }
}