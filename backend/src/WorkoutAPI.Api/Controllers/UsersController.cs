using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Application.Abstractions;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Application.Services;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
    {
        var user = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
    {
        var user = await _userService.UpdateUserAsync(id, request);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(Guid id)
    {
        await _userService.RestoreUserAsync(id);
        return Ok();
    }
}