using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class Member : BaseEntity {
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime MembershipStartDate { get; set; }
    public DateTime MembershipEndDate { get; set; }
    public MembershipType MembershipType { get; set; }
    public bool IsActiveMember { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<WorkoutSession> AttendedSessions { get; set; } = new List<WorkoutSession>();
    public virtual ICollection<UserWorkoutPlan> EnrolledWorkoutPlans { get; set; } = new List<UserWorkoutPlan>();
}


