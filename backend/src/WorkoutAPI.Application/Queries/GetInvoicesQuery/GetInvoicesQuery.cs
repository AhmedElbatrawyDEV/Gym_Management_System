using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Queries.GetInvoicesQuery;
public class GetInvoicesQuery : BasePaginatedQuery<InvoiceDto>
{
    public Guid? UserId { get; init; }
    public InvoiceStatus? Status { get; init; }
}