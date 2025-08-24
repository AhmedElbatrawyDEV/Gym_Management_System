using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class UserCredentials : BaseEntity {
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsLocked { get; set; } = false;
    public int FailedLoginAttempts { get; set; } = 0;
}

