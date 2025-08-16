using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;
public record CreateUserRequest(string FirstName, string LastName, string Email, string Password, Role Role);
public record UpdateUserRequest(string FirstName, string LastName, bool IsActive, string? Password);
public record UserResponse(Guid Id, string FullName, string Email, Role Role, bool IsActive, int Age);
public record UserProfileResponse(Guid Id, string FirstName, string LastName, string Email, int Age);