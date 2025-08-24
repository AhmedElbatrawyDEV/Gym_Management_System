using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Entities;

public class UserWorkoutPlan : BaseEntity {
    public Guid UserId { get; set; }
    public Guid WorkoutPlanId { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public int CompletedSessions { get; set; } = 0;

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;
}

