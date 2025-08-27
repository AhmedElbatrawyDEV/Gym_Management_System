using MediatR;

namespace WorkoutAPI.Application.Common.Models;

public abstract class BaseQuery<TResponse> : IRequest<Result<TResponse>>
{
}
public abstract class BasePagedQuery<TResponse> : BaseQuery<PaginatedResult<TResponse>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
