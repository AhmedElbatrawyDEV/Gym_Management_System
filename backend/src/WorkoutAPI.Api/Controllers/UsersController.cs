using global::WorkoutAPI.Api.Common;
using global::WorkoutAPI.Application.Queries.GetAttendanceRecords;
using global::WorkoutAPI.Application.Queries.GetUserById;
using global::WorkoutAPI.Application.Queries.GetUsersQuery;
using global::WorkoutAPI.Application.Queries.GetUserSubscriptions;
using global::WorkoutAPI.Application.Queries.GetUserWorkoutSessions;
using global::WorkoutAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Api.Models;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Application.Queries.GetUserPayments;


namespace WorkoutAPI.Api.Controllers;

// Updated Users Controller with Pagination

/// <summary>
/// Users management endpoints
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : BaseController
{
    /// <summary>
    /// Get all users with pagination and optional filtering
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="searchTerm">Search term for filtering users</param>
    /// <param name="status">User status filter</param>
    /// <returns>Paginated list of users</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<UserDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] UserStatus? status = null)
    {
        // Validate pagination parameters
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Limit max page size

        var query = new GetUsersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Status = status
        };

        var result = await Mediator.Send(query);
        return HandlePaginatedResult(result);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    /// <summary>
    /// Get user attendance records with pagination
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="fromDate">Start date filter</param>
    /// <param name="toDate">End date filter</param>
    /// <returns>Paginated user attendance records</returns>
    [HttpGet("{userId:guid}/attendance")]
    [ProducesResponseType(typeof(PaginatedResponse<AttendanceRecordDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUserAttendance(
        Guid userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = new GetAttendanceRecordsQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            FromDate = fromDate,
            ToDate = toDate
        };

        var result = await Mediator.Send(query);
        return HandlePaginatedResult(result);
    }

    /// <summary>
    /// Get user payments with pagination
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <returns>Paginated user payment history</returns>
    [HttpGet("{userId:guid}/payments")]
    [ProducesResponseType(typeof(Result<List<PaymentDto>>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUserPayments(
        Guid userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = new GetUserPaymentsQuery
        {
            UserId = userId,
        };

        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    /// <summary>
    /// Get user subscriptions (non-paginated - typically small list)
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="activeOnly">Filter for active subscriptions only</param>
    /// <returns>User subscriptions</returns>
    [HttpGet("{userId:guid}/subscriptions")]
    [ProducesResponseType(typeof(Result<List<UserSubscriptionDto>>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUserSubscriptions(
        Guid userId,
        [FromQuery] bool? activeOnly = null)
    {
        var query = new GetUserSubscriptionsQuery
        {
            UserId = userId,
            ActiveOnly = activeOnly
        };

        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    /// <summary>
    /// Get user workout sessions (non-paginated - can add pagination later if needed)
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User workout sessions</returns>
    [HttpGet("{userId:guid}/workout-sessions")]
    [ProducesResponseType(typeof(List<WorkoutSessionDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUserWorkoutSessions(Guid userId)
    {
        var query = new GetUserWorkoutSessionsQuery { UserId = userId };
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }
}



