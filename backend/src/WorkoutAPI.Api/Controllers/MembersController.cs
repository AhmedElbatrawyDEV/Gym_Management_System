using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Application.Services;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly ILogger<MembersController> _logger;

    public MembersController(IMemberService memberService, ILogger<MembersController> logger)
    {
        _memberService = memberService;
        _logger = logger;
    }

    /// <summary>
    /// Get all active members
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetMembers()
    {
        try
        {
            var members = await _memberService.GetActiveMembersAsync();
            return Ok(members);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting members");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get member by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemberResponse>> GetMember(Guid id)
    {
        try
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound($"Member with ID {id} not found");
            }

            return Ok(member);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting member {MemberId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get member by user ID
    /// </summary>
    [HttpGet("by-user/{userId:guid}")]
    public async Task<ActionResult<MemberResponse>> GetMemberByUserId(Guid userId)
    {
        try
        {
            var member = await _memberService.GetMemberByUserIdAsync(userId);
            if (member == null)
            {
                return NotFound($"Member for user ID {userId} not found");
            }

            return Ok(member);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting member by user ID {UserId}", userId);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get members by membership type
    /// </summary>
    [HttpGet("by-membership-type/{membershipType}")]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetMembersByMembershipType(MembershipType membershipType)
    {
        try
        {
            var members = await _memberService.GetMembersByMembershipTypeAsync(membershipType);
            return Ok(members);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting members by membership type {MembershipType}", membershipType);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get members with expiring memberships
    /// </summary>
    [HttpGet("expiring")]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetExpiringMemberships([FromQuery] int daysFromNow = 30)
    {
        try
        {
            var members = await _memberService.GetExpiringMembershipsAsync(daysFromNow);
            return Ok(members);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting expiring memberships");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Create a new member
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MemberResponse>> CreateMember([FromBody] CreateMemberRequest request)
    {
        try
        {
            var member = await _memberService.CreateMemberAsync(request);
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument while creating member");
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while creating member");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating member");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Update an existing member
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MemberResponse>> UpdateMember(Guid id, [FromBody] UpdateMemberRequest request)
    {
        try
        {
            var member = await _memberService.UpdateMemberAsync(id, request);
            return Ok(member);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Member not found for update: {MemberId}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating member {MemberId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Delete a member (soft delete)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMember(Guid id)
    {
        try
        {
            var result = await _memberService.DeleteMemberAsync(id);
            if (!result)
            {
                return NotFound($"Member with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting member {MemberId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}

