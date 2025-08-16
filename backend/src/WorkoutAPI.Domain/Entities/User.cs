using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;
namespace WorkoutAPI.Domain.Entities;
public class User : BaseEntity
{
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FullName => $"{FirstName} {LastName}".Trim();
    public Role Role { get; set; } = Role.Member;
    public bool IsActive { get; set; } = true;
    public DateTime? DateOfBirth { get; set; }
    public int Age => DateOfBirth is null ? 0 :
        DateTime.Today.Year - DateOfBirth.Value.Year - (DateTime.Today.DayOfYear < DateOfBirth.Value.DayOfYear ? 1 : 0);
    public MemberProfile? MemberProfile { get; set; }
    public TrainerProfile? TrainerProfile { get; set; }
}