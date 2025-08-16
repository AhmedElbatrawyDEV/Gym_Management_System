using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class ClassBooking : BaseEntity
{
    public Guid GymClassId { get; set; }
    public Guid MemberProfileId { get; set; }
    public bool Attended { get; set; } = false;
}