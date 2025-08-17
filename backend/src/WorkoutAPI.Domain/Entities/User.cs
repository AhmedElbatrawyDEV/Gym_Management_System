using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int Status { get; set; } 

    public string MembershipNumber { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;

    // Navigation Properties
    public virtual ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
    public virtual ICollection<UserWorkoutPlan> UserWorkoutPlans { get; set; } = new List<UserWorkoutPlan>();
    
    // Computed Properties
    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.UtcNow.Year - DateOfBirth.Year - (DateTime.UtcNow.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
}

