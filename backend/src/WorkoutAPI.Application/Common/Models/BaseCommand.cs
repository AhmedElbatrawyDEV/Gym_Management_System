using MediatR;

namespace WorkoutAPI.Application.Common.Models;

public abstract class BaseCommand : IRequest<Result>
{
}

public abstract class BaseCommand<TResponse> : IRequest<Result<TResponse>>
{
}
