using Mapster;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Services;

public interface IAuthenticationService {
    Task<AuthenticationResponse> LoginAsync(LoginRequest request);
    Task<UserResponse> RegisterAsync(RegisterRequest request);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
    Task<bool> ResetPasswordAsync(string email);
    Task<bool> LockUserAsync(Guid userId);
    Task<bool> UnlockUserAsync(Guid userId);
}

public class AuthenticationService : IAuthenticationService {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthenticationService> _logger;
    private const int MaxFailedAttempts = 5;

    public AuthenticationService(IUnitOfWork unitOfWork, ILogger<AuthenticationService> logger) {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request) {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var credentials = await _unitOfWork.UserCredentials.GetByEmailAsync(request.Email);
        if (credentials == null || !credentials.User.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (credentials.IsLocked)
        {
            throw new UnauthorizedAccessException("Account is locked. Please contact administrator");
        }

        if (!VerifyPassword(request.Password, credentials.PasswordHash, credentials.Salt))
        {
            await _unitOfWork.UserCredentials.IncrementFailedLoginAttemptsAsync(credentials.UserId);

            if (credentials.FailedLoginAttempts + 1 >= MaxFailedAttempts)
            {
                await _unitOfWork.UserCredentials.LockUserAsync(credentials.UserId);
                _logger.LogWarning("User account locked due to too many failed attempts: {Email}", request.Email);
            }

            throw new UnauthorizedAccessException("Invalid email or password");
        }

        await _unitOfWork.UserCredentials.ResetFailedLoginAttemptsAsync(credentials.UserId);
        await _unitOfWork.UserCredentials.UpdateLastLoginAsync(credentials.UserId);

        _logger.LogInformation("User logged in successfully: {Email}", request.Email);

        return new AuthenticationResponse(
            credentials.User.Adapt<UserResponse>(),
            credentials.Role.ToString(),
            GenerateToken(credentials.User, credentials.Role)
        );
    }

    public async Task<UserResponse> RegisterAsync(RegisterRequest request) {
        _logger.LogInformation("Registering new user with email: {Email}", request.Email);

        // Check if user already exists
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with email {request.Email} already exists");
        }

        // Create user
        var user = new User {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            ProfileImageUrl = request.ProfileImageUrl
        };

        await _unitOfWork.Users.AddAsync(user);

        // Create credentials
        var (hash, salt) = HashPassword(request.Password);
        var credentials = new UserCredentials {
            UserId = user.Id,
            PasswordHash = hash,
            Salt = salt,
            Role = request.Role
        };

        await _unitOfWork.UserCredentials.AddAsync(credentials);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User registered successfully with ID: {UserId}", user.Id);

        return user.Adapt<UserResponse>();
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request) {
        var credentials = await _unitOfWork.UserCredentials.GetByUserIdAsync(userId);
        if (credentials == null)
        {
            return false;
        }

        if (!VerifyPassword(request.CurrentPassword, credentials.PasswordHash, credentials.Salt))
        {
            throw new UnauthorizedAccessException("Current password is incorrect");
        }

        var (hash, salt) = HashPassword(request.NewPassword);
        credentials.PasswordHash = hash;
        credentials.Salt = salt;
        credentials.SetUpdated();

        _unitOfWork.UserCredentials.Update(credentials);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ResetPasswordAsync(string email) {
        var credentials = await _unitOfWork.UserCredentials.GetByEmailAsync(email);
        if (credentials == null)
        {
            return false;
        }

        // Generate temporary password
        var tempPassword = GenerateTemporaryPassword();
        var (hash, salt) = HashPassword(tempPassword);

        credentials.PasswordHash = hash;
        credentials.Salt = salt;
        credentials.SetUpdated();

        _unitOfWork.UserCredentials.Update(credentials);
        await _unitOfWork.SaveChangesAsync();

        // TODO: Send email with temporary password
        _logger.LogInformation("Password reset for user: {Email}. Temporary password: {TempPassword}", email, tempPassword);

        return true;
    }

    public async Task<bool> LockUserAsync(Guid userId) {
        return await _unitOfWork.UserCredentials.LockUserAsync(userId);
    }

    public async Task<bool> UnlockUserAsync(Guid userId) {
        return await _unitOfWork.UserCredentials.UnlockUserAsync(userId);
    }

    private static (string hash, string salt) HashPassword(string password) {
        var salt = GenerateSalt();
        var hash = HashPasswordWithSalt(password, salt);
        return (hash, salt);
    }

    private static bool VerifyPassword(string password, string hash, string salt) {
        var computedHash = HashPasswordWithSalt(password, salt);
        return hash == computedHash;
    }

    private static string HashPasswordWithSalt(string password, string salt) {
        using var sha256 = SHA256.Create();
        var saltedPassword = password + salt;
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashedBytes);
    }

    private static string GenerateSalt() {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string GenerateTemporaryPassword() {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static string GenerateToken(User user, UserRole role) {
        // TODO: Implement JWT token generation
        return $"token_{user.Id}_{role}_{DateTime.UtcNow.Ticks}";
    }
}


public record RegisterRequest(string FirstName, string LastName, string Email, string? PhoneNumber, DateTime DateOfBirth, Gender Gender, string? ProfileImageUrl, string Password, UserRole Role);
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
public record AuthenticationResponse(UserResponse User, string Role, string Token);

