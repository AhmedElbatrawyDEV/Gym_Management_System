using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs {
    public record CreateAdminRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        AdminRole Role = AdminRole.Admin
    );
    public record UpdateAdminRequest(
    string FirstName,
    string LastName,
    string Email,
    AdminRole Role,
    bool IsActive
);

    public record AdminResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        AdminRole Role,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? LastLoginAt
    );

    public record LoginRequest(
        string Email,
        string Password
    );

    public record LoginResponse(
        string Token,
        AdminResponse Admin,
        DateTime ExpiresAt
    );

    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword
    );
}
