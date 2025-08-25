
// Entities
using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Entities;

public class Schedule : Entity<Schedule, Guid>
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Guid? TrainerId { get; private set; }
    public Guid? WorkoutPlanId { get; private set; }
    public int Capacity { get; private set; }
    public int EnrolledCount { get; private set; }

    // Navigation properties
    public virtual Trainer? Trainer { get; set; }
    public virtual WorkoutPlan? WorkoutPlan { get; set; }

    private Schedule() { } // EF Core

    public static Schedule CreateNew(string title, string description, DateTime startTime,
                                   DateTime endTime, int capacity, Guid? trainerId = null, Guid? workoutPlanId = null)
    {
        return new Schedule
        {
            Id = Guid.NewGuid(),
            Title = title ?? throw new ArgumentNullException(nameof(title)),
            Description = description ?? throw new ArgumentNullException(nameof(description)),
            StartTime = startTime,
            EndTime = endTime > startTime ? endTime : throw new ArgumentException("End time must be after start time"),
            Capacity = capacity > 0 ? capacity : throw new ArgumentException("Capacity must be positive"),
            TrainerId = trainerId,
            WorkoutPlanId = workoutPlanId,
            EnrolledCount = 0
        };
    }

    public void Enroll()
    {
        if (EnrolledCount >= Capacity)
            throw new InvalidOperationException("Schedule is at full capacity");

        EnrolledCount++;
    }

    public void Unenroll()
    {
        if (EnrolledCount <= 0)
            throw new InvalidOperationException("No one to unenroll");

        EnrolledCount--;
    }

    public void UpdateDetails(string title, string description)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public void UpdateTiming(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time");

        StartTime = startTime;
        EndTime = endTime;
    }

    public void AssignTrainer(Guid trainerId)
    {
        TrainerId = trainerId;
    }

    public void RemoveTrainer()
    {
        TrainerId = null;
    }

    public void AssignWorkoutPlan(Guid workoutPlanId)
    {
        WorkoutPlanId = workoutPlanId;
    }

    public void RemoveWorkoutPlan()
    {
        WorkoutPlanId = null;
    }

    public bool HasAvailableSpots => EnrolledCount < Capacity;
    public int AvailableSpots => Math.Max(0, Capacity - EnrolledCount);
    public TimeSpan Duration => EndTime - StartTime;
    public bool IsInProgress => DateTime.UtcNow >= StartTime && DateTime.UtcNow <= EndTime;
    public bool IsCompleted => DateTime.UtcNow > EndTime;
    public bool IsUpcoming => DateTime.UtcNow < StartTime;
}
