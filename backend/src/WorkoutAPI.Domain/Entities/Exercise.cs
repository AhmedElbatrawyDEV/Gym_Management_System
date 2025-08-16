using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class Exercise : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? TargetMuscle { get; set; }
    public string? Equipment { get; set; }
    public ICollection<Translation> Translations { get; set; }
}
