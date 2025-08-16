using WorkoutAPI.Application.DTOs;
namespace WorkoutAPI.Application.Abstractions;
public interface IUserService
{
    Task<List<UserResponse>> ListAsync();
    Task<UserResponse?> GetAsync(Guid id);
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request);
    Task DeleteAsync(Guid id);
}