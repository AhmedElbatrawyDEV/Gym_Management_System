
// Entities
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class ClassBooking : Entity<ClassBooking, Guid>
{
    public Guid UserId { get; private set; }
    public Guid ClassScheduleId { get; private set; }
    public DateTime BookingDate { get; private set; }
    public BookingStatus Status { get; private set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ClassSchedule ClassSchedule { get; set; } = null!;

    private ClassBooking() { } // EF Core

    public static ClassBooking CreateNew(Guid userId, Guid classScheduleId)
    {
        return new ClassBooking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ClassScheduleId = classScheduleId,
            BookingDate = DateTime.UtcNow,
            Status = BookingStatus.Confirmed
        };
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed booking");

        Status = BookingStatus.Cancelled;
    }

    public void CheckIn()
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Can only check in confirmed bookings");

        Status = BookingStatus.CheckedIn;
    }

    public void MarkAsNoShow()
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Can only mark confirmed bookings as no-show");

        Status = BookingStatus.NoShow;
    }

    public void Complete()
    {
        if (Status != BookingStatus.CheckedIn)
            throw new InvalidOperationException("Can only complete checked-in bookings");

        Status = BookingStatus.Completed;
    }

    public void Waitlist()
    {
        Status = BookingStatus.Waitlisted;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Waitlisted && Status != BookingStatus.Pending)
            throw new InvalidOperationException("Can only confirm waitlisted or pending bookings");

        Status = BookingStatus.Confirmed;
    }
}
