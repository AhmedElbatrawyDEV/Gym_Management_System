
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Domain.Entities;

// Additional supporting entities would follow the same pattern...
public class Trainer : Entity<Trainer, Guid>
{
    public Guid UserId { get; private set; }
    public string Specialization { get; private set; } = string.Empty;
    public string Certification { get; private set; } = string.Empty;
    public Money HourlyRate { get; private set; }
    public bool IsAvailable { get; private set; } = true;

    private Trainer() { } // EF Core

    public static Trainer CreateNew(Guid userId, string specialization, string certification, Money hourlyRate)
    {
        return new Trainer
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Specialization = specialization ?? throw new ArgumentNullException(nameof(specialization)),
            Certification = certification ?? throw new ArgumentNullException(nameof(certification)),
            HourlyRate = hourlyRate ?? throw new ArgumentNullException(nameof(hourlyRate))
        };
    }

    public void UpdateHourlyRate(Money newRate)
    {
        HourlyRate = newRate ?? throw new ArgumentNullException(nameof(newRate));
    }

    public void SetAvailability(bool isAvailable) => IsAvailable = isAvailable;
}