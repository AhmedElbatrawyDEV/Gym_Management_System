using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Interfaces;

public interface IUserCredentialsRepository : IRepository<UserCredentials>
{
    Task<UserCredentials?> GetByUserIdAsync(Guid userId);
    Task<UserCredentials?> GetByEmailAsync(string email);
    Task<IEnumerable<UserCredentials>> GetByRoleAsync(UserRole role);
    Task<bool> UpdateLastLoginAsync(Guid userId);
    Task<bool> IncrementFailedLoginAttemptsAsync(Guid userId);
    Task<bool> ResetFailedLoginAttemptsAsync(Guid userId);
    Task<bool> LockUserAsync(Guid userId);
    Task<bool> UnlockUserAsync(Guid userId);
}

