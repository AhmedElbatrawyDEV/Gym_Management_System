using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.ValueObjects;

public class DateRange : ValueObject {
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    private DateRange() { } // EF Core

    public DateRange(DateTime startDate, DateTime endDate) {
        if (startDate >= endDate)
            throw new ArgumentException("Start date must be before end date");

        StartDate = startDate;
        EndDate = endDate;
    }

    public int DurationInDays => (EndDate - StartDate).Days;
    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    public bool Contains(DateTime date) => date >= StartDate && date <= EndDate;

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return StartDate;
        yield return EndDate;
    }
}

