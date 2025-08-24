using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Entities;

public class Schedule : BaseEntity {
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid? TrainerId { get; set; }
    public Trainer? Trainer { get; set; }
    public Guid? WorkoutPlanId { get; set; }
    public WorkoutPlan? WorkoutPlan { get; set; }
    public int Capacity { get; set; }
    public int EnrolledCount { get; set; }
}


