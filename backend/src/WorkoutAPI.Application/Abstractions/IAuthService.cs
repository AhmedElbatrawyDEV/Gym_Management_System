using WorkoutAPI.Application.DTOs;
namespace WorkoutAPI.Application.Abstractions;
public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}