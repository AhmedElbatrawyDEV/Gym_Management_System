using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class GymClass : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid? TrainerProfileId { get; set; }
    public int Capacity { get; set; } = 20;
}