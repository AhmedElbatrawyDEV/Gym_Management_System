using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;
namespace WorkoutAPI.Domain.Entities;
public class MemberProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public string? Phone { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string MembershipNumber { get; set; } = default!;
}