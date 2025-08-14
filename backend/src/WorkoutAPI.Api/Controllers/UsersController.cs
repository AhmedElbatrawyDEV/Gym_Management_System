using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Application.Services;

namespace WorkoutAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _createUserValidator;
    private readonly IValidator<UpdateUserRequest> _updateUserValidator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IUserService userService,
        IValidator<CreateUserRequest> createUserValidator,
        IValidator<UpdateUserRequest> updateUserValidator,
        ILogger<UsersController> logger)
    {
        _userService = userService;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all active users
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
        try
        {
            var users = await _userService.GetActiveUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting users");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user {UserId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get user profile with workout plans and recent sessions
    /// </summary>
    [HttpGet("{id:guid}/profile")]
    public async Task<ActionResult<UserProfileResponse>> GetUserProfile(Guid id)
    {
        try
        {
            var userProfile = await _userService.GetUserProfileAsync(id);
            if (userProfile == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            return Ok(userProfile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user profile {UserId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<UserResponse>> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"User with email {email} not found");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user by email {Email}", email);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            // Validate request
            var validationResult = await _createUserValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while creating user");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            // Validate request
            var validationResult = await _updateUserValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var user = await _userService.UpdateUserAsync(id, request);
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "User not found for update: {UserId}", id);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while updating user");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user {UserId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Delete a user (soft delete)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound($"User with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user {UserId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Check if user exists by email
    /// </summary>
    [HttpGet("exists/{email}")]
    public async Task<ActionResult<bool>> UserExists(string email)
    {
        try
        {
            var exists = await _userService.UserExistsAsync(email);
            return Ok(exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking user existence for email {Email}", email);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}

