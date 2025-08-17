// AdminController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Application.Services;

namespace WorkoutAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateAdminRequest request)
        {
            try
            {
                var result = await _adminService.CreateAdminAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _adminService.LoginAsync(request.Email, request.Password);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(admins);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(Guid id, UpdateAdminRequest request)
        {
            try
            {
                var result = await _adminService.UpdateAdminAsync(id, request);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            try
            {
                await _adminService.DeleteAdminAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordRequest request)
        {
            try
            {
                await _adminService.ChangePasswordAsync(id, request.CurrentPassword, request.NewPassword);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest(new { message = "Current password is incorrect" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}