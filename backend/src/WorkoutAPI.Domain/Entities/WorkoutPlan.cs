using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class WorkoutPlan : BaseEntity
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? TrainerProfileId { get; set; }
    public TrainerProfile? TrainerProfile { get; set; }
    public Guid? MemberProfileId { get; set; }
    public MemberProfile? MemberProfile { get; set; }
    public ICollection<WorkoutExercise> Exercises { get; set; } = new List<WorkoutExercise>();
    public ICollection<Translation> Translations { get; set; } = new List<Translation>();
}