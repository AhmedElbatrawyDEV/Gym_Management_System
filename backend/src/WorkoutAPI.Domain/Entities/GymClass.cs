using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;
namespace WorkoutAPI.Domain.Entities;
public class GymClass : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? InstructorId { get; set; }
    public int MaxCapacity { get; set; }
    public TimeSpan Duration { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public User? Instructor { get; set; }
    public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
}