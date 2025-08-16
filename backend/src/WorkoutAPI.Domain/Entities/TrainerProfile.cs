using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class TrainerProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public string? Bio { get; set; }
    public string? Specialty { get; set; }
}