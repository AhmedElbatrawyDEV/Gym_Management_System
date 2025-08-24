using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkoutAPI.Api.Controllers;

// SubscriptionController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionsController : ControllerBase {
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService) {
        _subscriptionService = subscriptionService;
    }

    [HttpGet("plans")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSubscriptionPlans() {
        var plans = await _subscriptionService.GetAllPlansAsync();
        return Ok(plans);
    }

    [HttpPost("plans")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> CreateSubscriptionPlan(CreateSubscriptionPlanRequest request) {
        var result = await _subscriptionService.CreatePlanAsync(request);
        return CreatedAtAction(nameof(GetSubscriptionPlans), new { id = result.Id }, result);
    }

    [HttpPut("plans/{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateSubscriptionPlan(Guid id, UpdateSubscriptionPlanRequest request) {
        try
        {
            var result = await _subscriptionService.UpdatePlanAsync(id, request);
            return Ok(result);
        } catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("users/{userId}/assign")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> AssignSubscription(Guid userId, AssignSubscriptionRequest request) {
        try
        {
            var result = await _subscriptionService.AssignSubscriptionAsync(userId, request);
            return Ok(result);
        } catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserSubscriptions(Guid userId) {
        var subscriptions = await _subscriptionService.GetUserSubscriptionsAsync(userId);
        return Ok(subscriptions);
    }

    [HttpPut("users/{id}/extend")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> ExtendSubscription(Guid id, ExtendSubscriptionRequest request) {
        try
        {
            var result = await _subscriptionService.ExtendSubscriptionAsync(id, request.ExtensionDays);
            return Ok(result);
        } catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("users/{id}/cancel")]
    public async Task<IActionResult> CancelSubscription(Guid id) {
        try
        {
            await _subscriptionService.CancelSubscriptionAsync(id);
            return Ok(new { message = "Subscription cancelled successfully" });
        } catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
