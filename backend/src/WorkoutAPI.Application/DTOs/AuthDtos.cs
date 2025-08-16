using WorkoutAPI.Domain.Enums;
namespace WorkoutAPI.Application.DTOs;
public record RegisterRequest(string Email, string Password, string FirstName, string LastName, Role Role);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string AccessToken, string Email, string FullName, Role Role);