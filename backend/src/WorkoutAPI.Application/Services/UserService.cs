using WorkoutAPI.Application.Abstractions;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Interfaces;

namespace WorkoutAPI.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        IRepository<User> userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UserDto> GetUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return MapToDto(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.ListAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto> CreateUserAsync(UserCreateRequest request)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            ProfileImageUrl = request.ProfileImageUrl
        };

        await _userRepository.AddAsync(user, _currentUserService.UserId);
        return MapToDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UserUpdateRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.DateOfBirth = request.DateOfBirth;
        user.Gender = request.Gender;
        user.ProfileImageUrl = request.ProfileImageUrl;

        await _userRepository.UpdateAsync(user, _currentUserService.UserId);
        return MapToDto(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id, _currentUserService.UserId);
    }

    public async Task RestoreUserAsync(Guid id)
    {
        await _userRepository.RestoreAsync(id, _currentUserService.UserId);
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender.ToString(),
            ProfileImageUrl = user.ProfileImageUrl,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}