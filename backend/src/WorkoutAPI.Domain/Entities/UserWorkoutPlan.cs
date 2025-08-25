
// Entities
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class UserWorkoutPlan : Entity<UserWorkoutPlan, Guid>
{
    public Guid UserId { get; private set; }
    public Guid WorkoutPlanId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public WorkoutPlanStatus Status { get; private set; }
    public decimal Progress { get; private set; } = 0;
    public Guid AssignedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;
    public virtual User AssignedByUser { get; set; } = null!;

    private UserWorkoutPlan() { } // EF Core

    public static UserWorkoutPlan CreateNew(Guid userId, Guid workoutPlanId, DateTime startDate, Guid assignedBy)
    {
        return new UserWorkoutPlan
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            WorkoutPlanId = workoutPlanId,
            StartDate = startDate,
            Status = WorkoutPlanStatus.Active,
            AssignedBy = assignedBy,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Complete()
    {
        if (Status != WorkoutPlanStatus.Active)
            throw new InvalidOperationException("Can only complete active workout plans");

        Status = WorkoutPlanStatus.Inactive;
        EndDate = DateTime.UtcNow;
        Progress = 100;
    }

    public void Pause()
    {
        if (Status != WorkoutPlanStatus.Active)
            throw new InvalidOperationException("Can only pause active workout plans");

        Status = WorkoutPlanStatus.Draft; // Using Draft as paused status
    }

    public void Resume()
    {
        if (Status != WorkoutPlanStatus.Draft)
            throw new InvalidOperationException("Can only resume paused workout plans");

        Status = WorkoutPlanStatus.Active;
    }

    public void UpdateProgress(decimal progressPercentage)
    {
        if (progressPercentage < 0 || progressPercentage > 100)
            throw new ArgumentException("Progress must be between 0 and 100");

        Progress = progressPercentage;

        if (Progress >= 100 && Status == WorkoutPlanStatus.Active)
        {
            Complete();
        }
    }

    public bool IsActive => Status == WorkoutPlanStatus.Active;
    public bool IsCompleted => Status == WorkoutPlanStatus.Inactive && EndDate.HasValue;
    public TimeSpan? Duration => EndDate?.Subtract(StartDate);
}
