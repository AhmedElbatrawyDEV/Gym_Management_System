using MediatR;

namespace WorkoutAPI.Application.Common.Models;

public abstract class BaseQuery<TResponse> : IRequest<Result<TResponse>>
{
}

public abstract class BasePaginatedQuery<TResponse> : IRequest<PaginatedResult<TResponse>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}