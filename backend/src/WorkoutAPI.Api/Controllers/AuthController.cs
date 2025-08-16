using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutAPI.Application.Services;

namespace WorkoutAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthenticationService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Login failed for email: {Email}", request.Email);
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for email: {Email}", request.Email);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Login), result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed for email: {Email}", request.Email);
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during registration for email: {Email}", request.Email);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var result = await _authService.ChangePasswordAsync(Guid.Parse(userId), request);
            return Ok(new { message = "Password changed successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Password change failed");
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during password change");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            var result = await _authService.ResetPasswordAsync(request.Email);
            if (!result)
            {
                return NotFound("User not found");
            }

            return Ok(new { message = "Password reset email sent" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during password reset for email: {Email}", request.Email);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [Authorize]
    [HttpPost("lock-user/{userId:guid}")]
    public async Task<IActionResult> LockUser(Guid userId)
    {
        try
        {
            var result = await _authService.LockUserAsync(userId);
            if (!result)
            {
                return NotFound("User not found");
            }

            return Ok(new { message = "User locked successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while locking user: {UserId}", userId);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [Authorize]
    [HttpPost("unlock-user/{userId:guid}")]
    public async Task<IActionResult> UnlockUser(Guid userId)
    {
        try
        {
            var result = await _authService.UnlockUserAsync(userId);
            if (!result)
            {
                return NotFound("User not found");
            }

            return Ok(new { message = "User unlocked successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while unlocking user: {UserId}", userId);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}

public record ResetPasswordRequest(string Email);