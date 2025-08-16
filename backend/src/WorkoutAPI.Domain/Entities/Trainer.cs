using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class Trainer : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Specialization { get; set; } = string.Empty;
    public string Certification { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<WorkoutSession> ScheduledSessions { get; set; } = new List<WorkoutSession>();
}


