using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;


namespace WorkoutAPI.Domain.Entities
{
    public class Admin : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public AdminRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockedUntil { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public bool IsLocked => LockedUntil.HasValue && LockedUntil > DateTime.UtcNow;
    }
}