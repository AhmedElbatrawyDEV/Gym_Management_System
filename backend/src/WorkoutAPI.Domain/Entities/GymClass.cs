
// Entities
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class GymClass : Entity<GymClass, Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid? InstructorId { get; private set; }
    public int MaxCapacity { get; private set; }
    public TimeSpan Duration { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public virtual User? Instructor { get; set; }
    public virtual ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();

    private GymClass() { } // EF Core

    public static GymClass CreateNew(string name, string description, int maxCapacity,
                                   TimeSpan duration, DifficultyLevel difficulty, Guid? instructorId = null)
    {
        return new GymClass
        {
            Id = Guid.NewGuid(),
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            Description = description ?? throw new ArgumentNullException(nameof(description)),
            MaxCapacity = maxCapacity > 0 ? maxCapacity : throw new ArgumentException("Max capacity must be positive"),
            Duration = duration,
            Difficulty = difficulty,
            InstructorId = instructorId
        };
    }

    public void UpdateDetails(string name, string description, int maxCapacity, TimeSpan duration)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        MaxCapacity = maxCapacity > 0 ? maxCapacity : throw new ArgumentException("Max capacity must be positive");
        Duration = duration;
    }

    public void AssignInstructor(Guid instructorId)
    {
        InstructorId = instructorId;
    }

    public void RemoveInstructor()
    {
        InstructorId = null;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
