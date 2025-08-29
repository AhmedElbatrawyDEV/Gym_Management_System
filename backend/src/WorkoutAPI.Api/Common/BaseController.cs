using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Api.Common;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value is null ? NoContent() : Ok(result.Value);
        }

        return BadRequest(new
        {
            StatusCode = 400,
            Title = "Bad Request",
            Detail = "One or more errors occurred.",
            Errors = result.Error
        });
    }

    protected IActionResult HandlePaginatedResult<T>(PaginatedResult<T> result)
    {
        if (result.IsSuccess)
        {
            var response = new
            {
                Items = result.Items,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalCount = result.TotalCount,
                HasPreviousPage = result.HasPreviousPage,
                HasNextPage = result.HasNextPage
            };

            // Add pagination headers
            Response.Headers.Add("X-Pagination-Current-Page", result.PageNumber.ToString());
            Response.Headers.Add("X-Pagination-Page-Size", result.PageSize.ToString());
            Response.Headers.Add("X-Pagination-Total-Count", result.TotalCount.ToString());
            Response.Headers.Add("X-Pagination-Total-Pages", result.TotalPages.ToString());
            Response.Headers.Add("X-Pagination-Has-Next", result.HasNextPage.ToString().ToLower());
            Response.Headers.Add("X-Pagination-Has-Previous", result.HasPreviousPage.ToString().ToLower());

            return Ok(response);
        }

        return BadRequest(new
        {
            StatusCode = 400,
            Title = "Bad Request",
            Detail = "One or more errors occurred.",
            Errors = result.Errors
        });
    }

    protected IActionResult Created<T>(T value, string actionName, object routeValues)
    {
        return CreatedAtAction(actionName, routeValues, value);
    }

    protected IActionResult Created<T>(T value)
    {
        return StatusCode(201, value);
    }
}
