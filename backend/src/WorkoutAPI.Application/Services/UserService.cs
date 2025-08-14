using Mapster;
using Microsoft.Extensions.Logging;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Services;

public interface IUserService
{
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);
    Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request);
    Task<UserResponse?> GetUserByIdAsync(Guid userId);
    Task<UserResponse?> GetUserByEmailAsync(string email);
    Task<UserProfileResponse?> GetUserProfileAsync(Guid userId);
    Task<IEnumerable<UserResponse>> GetActiveUsersAsync();
    Task<bool> DeleteUserAsync(Guid userId);
    Task<bool> UserExistsAsync(string email);
}

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        _logger.LogInformation("Creating new user with email: {Email}", request.Email);

        // Check if user already exists
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with email {request.Email} already exists");
        }

        // Check phone number if provided
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            var existingUserByPhone = await _unitOfWork.Users.GetByPhoneAsync(request.PhoneNumber);
            if (existingUserByPhone != null)
            {
                throw new InvalidOperationException($"User with phone number {request.PhoneNumber} already exists");
            }
        }

        var user = request.Adapt<User>();
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

        return user.Adapt<UserResponse>();
    }

    public async Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", userId);

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found");
        }

        // Check phone number if changed
        if (!string.IsNullOrEmpty(request.PhoneNumber) && request.PhoneNumber != user.PhoneNumber)
        {
            var existingUserByPhone = await _unitOfWork.Users.GetByPhoneAsync(request.PhoneNumber);
            if (existingUserByPhone != null && existingUserByPhone.Id != userId)
            {
                throw new InvalidOperationException($"User with phone number {request.PhoneNumber} already exists");
            }
        }

        // Update user properties
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.DateOfBirth = request.DateOfBirth;
        user.Gender = request.Gender;
        user.ProfileImageUrl = request.ProfileImageUrl;
        user.SetUpdated();

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User updated successfully with ID: {UserId}", userId);

        return user.Adapt<UserResponse>();
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        return user?.Adapt<UserResponse>();
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        return user?.Adapt<UserResponse>();
    }

    public async Task<UserProfileResponse?> GetUserProfileAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetUserWithWorkoutPlansAsync(userId);
        if (user == null) return null;

        var userWithSessions = await _unitOfWork.Users.GetUserWithSessionsAsync(userId);
        if (userWithSessions != null)
        {
            user.WorkoutSessions = userWithSessions.WorkoutSessions;
        }

        return user.Adapt<UserProfileResponse>();
    }

    public async Task<IEnumerable<UserResponse>> GetActiveUsersAsync()
    {
        var users = await _unitOfWork.Users.GetActiveUsersAsync();
        return users.Adapt<IEnumerable<UserResponse>>();
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", userId);

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // Soft delete by setting IsActive to false
        user.IsActive = false;
        user.SetUpdated();

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User soft deleted successfully with ID: {UserId}", userId);

        return true;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _unitOfWork.Users.ExistsAsync(u => u.Email == email && u.IsActive);
    }
}

