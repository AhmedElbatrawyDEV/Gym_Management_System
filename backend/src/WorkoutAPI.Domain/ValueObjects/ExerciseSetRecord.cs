using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.ValueObjects;

public class ExerciseSetRecord : ValueObject
{
    public int SetNumber { get; private set; }
    public int? Reps { get; private set; }
    public decimal? Weight { get; private set; }
    public TimeSpan? Duration { get; private set; }
    public int? Distance { get; private set; }

    private ExerciseSetRecord() { } // EF Core

    public ExerciseSetRecord(int setNumber, int? reps = null, decimal? weight = null,
                           TimeSpan? duration = null, int? distance = null)
    {
        if (setNumber <= 0)
            throw new ArgumentException("Set number must be positive", nameof(setNumber));

        SetNumber = setNumber;
        Reps = reps;
        Weight = weight;
        Duration = duration;
        Distance = distance;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SetNumber;
        yield return Reps ?? 0;
        yield return Weight ?? 0;
        yield return Duration?.TotalSeconds ?? 0;
        yield return Distance ?? 0;
    }
}

