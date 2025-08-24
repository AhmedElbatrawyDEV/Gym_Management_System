
// Entities
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums.WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class Member : Entity<Member, Guid> {
    public Guid UserId { get; private set; }
    public DateTime MembershipStartDate { get; private set; }
    public DateTime MembershipEndDate { get; private set; }
    public MembershipType MembershipType { get; private set; }
    public bool IsActiveMember { get; private set; } = true;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<WorkoutSession> AttendedSessions { get; set; } = new List<WorkoutSession>();
    public virtual ICollection<UserWorkoutPlan> EnrolledWorkoutPlans { get; set; } = new List<UserWorkoutPlan>();

    private Member() { } // EF Core

    public static Member CreateNew(Guid userId, MembershipType membershipType, DateTime startDate, DateTime endDate) {
        return new Member {
            Id = Guid.NewGuid(),
            UserId = userId,
            MembershipType = membershipType,
            MembershipStartDate = startDate,
            MembershipEndDate = endDate,
            IsActiveMember = true
        };
    }

    public void ExtendMembership(DateTime newEndDate) {
        if (newEndDate <= MembershipEndDate)
            throw new ArgumentException("New end date must be after current end date");

        MembershipEndDate = newEndDate;
        if (!IsActiveMember && DateTime.UtcNow <= newEndDate)
        {
            IsActiveMember = true;
        }
    }

    public void Deactivate() {
        IsActiveMember = false;
    }

    public void Reactivate() {
        if (DateTime.UtcNow <= MembershipEndDate)
        {
            IsActiveMember = true;
        }
        else
        {
            throw new InvalidOperationException("Cannot reactivate expired membership");
        }
    }

    public void ChangeMembershipType(MembershipType newType) {
        MembershipType = newType;
    }

    public bool IsMembershipExpired => DateTime.UtcNow > MembershipEndDate;
    public bool IsMembershipActive => IsActiveMember && !IsMembershipExpired;
    public TimeSpan MembershipDuration => MembershipEndDate - MembershipStartDate;
    public TimeSpan? TimeUntilExpiry => IsMembershipExpired ? null : MembershipEndDate - DateTime.UtcNow;
}
