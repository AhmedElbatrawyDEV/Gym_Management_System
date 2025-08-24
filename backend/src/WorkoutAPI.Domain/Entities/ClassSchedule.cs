
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums.WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class ClassSchedule : Entity<ClassSchedule, Guid> {
    public Guid GymClassId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int MaxCapacity { get; private set; }
    public int CurrentEnrollment { get; private set; }
    public ClassStatus Status { get; private set; }

    // Navigation properties
    public virtual GymClass GymClass { get; set; } = null!;
    public virtual ICollection<ClassBooking> Bookings { get; set; } = new List<ClassBooking>();

    private ClassSchedule() { } // EF Core

    public static ClassSchedule CreateNew(Guid gymClassId, DateTime startTime, DateTime endTime, int maxCapacity) {
        return new ClassSchedule {
            Id = Guid.NewGuid(),
            GymClassId = gymClassId,
            StartTime = startTime,
            EndTime = endTime,
            MaxCapacity = maxCapacity,
            CurrentEnrollment = 0,
            Status = ClassStatus.Scheduled
        };
    }

    public void EnrollMember() {
        if (CurrentEnrollment >= MaxCapacity)
            throw new InvalidOperationException("Class is at full capacity");

        CurrentEnrollment++;

        if (CurrentEnrollment >= MaxCapacity)
            Status = ClassStatus.Full;
    }

    public void UnenrollMember() {
        if (CurrentEnrollment <= 0)
            throw new InvalidOperationException("No members to unenroll");

        CurrentEnrollment--;

        if (Status == ClassStatus.Full && CurrentEnrollment < MaxCapacity)
            Status = ClassStatus.Scheduled;
    }

    public void StartClass() {
        if (Status != ClassStatus.Scheduled)
            throw new InvalidOperationException("Can only start scheduled classes");

        Status = ClassStatus.InProgress;
    }

    public void CompleteClass() {
        if (Status != ClassStatus.InProgress)
            throw new InvalidOperationException("Can only complete classes in progress");

        Status = ClassStatus.Completed;
    }

    public void CancelClass() {
        if (Status == ClassStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed class");

        Status = ClassStatus.Cancelled;
    }

    public bool HasAvailableSpots => CurrentEnrollment < MaxCapacity;
}
