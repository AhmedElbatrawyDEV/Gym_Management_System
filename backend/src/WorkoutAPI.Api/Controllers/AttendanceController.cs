using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkoutAPI.Api.Controllers;

// AttendanceController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase {
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService) {
        _attendanceService = attendanceService;
    }

    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn(CheckInRequest request) {
        try
        {
            var result = await _attendanceService.CheckInAsync(request.UserId, request.ActivityType);
            return Ok(result);
        } catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("check-out/{recordId}")]
    public async Task<IActionResult> CheckOut(Guid recordId) {
        try
        {
            var result = await _attendanceService.CheckOutAsync(recordId);
            return Ok(result);
        } catch (KeyNotFoundException)
        {
            return NotFound();
        } catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserAttendance(Guid userId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null) {
        var attendance = await _attendanceService.GetUserAttendanceAsync(userId, fromDate, toDate);
        return Ok(attendance);
    }

    [HttpGet("classes")]
    [AllowAnonymous]
    public async Task<IActionResult> GetGymClasses() {
        var classes = await _attendanceService.GetGymClassesAsync();
        return Ok(classes);
    }

    [HttpPost("classes")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> CreateGymClass(CreateGymClassRequest request) {
        var result = await _attendanceService.CreateGymClassAsync(request);
        return CreatedAtAction(nameof(GetGymClasses), new { id = result.Id }, result);
    }

    [HttpPost("classes/book/{scheduleId}")]
    public async Task<IActionResult> BookClass(Guid scheduleId, BookClassRequest request) {
        try
        {
            var result = await _attendanceService.BookClassAsync(request.UserId, scheduleId);
            return Ok(result);
        } catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("classes/book/{bookingId}")]
    public async Task<IActionResult> CancelBooking(Guid bookingId) {
        try
        {
            await _attendanceService.CancelBookingAsync(bookingId);
            return NoContent();
        } catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}