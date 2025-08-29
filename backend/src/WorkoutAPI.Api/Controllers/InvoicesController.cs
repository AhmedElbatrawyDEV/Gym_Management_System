using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Api.Common;
using WorkoutAPI.Api.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Application.Queries.GetInvoicesQuery;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Api.Controllers;
public class InvoicesController : BaseController
{
    /// <summary>
    /// Get invoices with pagination and optional filtering
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="userId">User ID filter</param>
    /// <param name="status">Invoice status filter</param>
    /// <returns>Paginated list of invoices</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<InvoiceDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetInvoices(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? userId = null,
        [FromQuery] InvoiceStatus? status = null)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = new GetInvoicesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = userId,
            Status = status
        };

        var result = await Mediator.Send(query);
        return HandlePaginatedResult(result);
    }
}