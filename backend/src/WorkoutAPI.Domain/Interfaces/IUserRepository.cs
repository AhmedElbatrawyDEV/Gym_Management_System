using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByPhoneAsync(string phoneNumber);
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<User?> GetUserWithWorkoutPlansAsync(Guid userId);
    Task<User?> GetUserWithSessionsAsync(Guid userId);
}

