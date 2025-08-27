using FluentValidation;
using MediatR;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            var errorMessages = failures.Select(f => f.ErrorMessage).ToArray();

            // If TResponse is a Result type, return validation failure
            if (typeof(TResponse).IsGenericType)
            {
                var genericTypeDefinition = typeof(TResponse).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Result<>))
                {
                    var resultType = typeof(TResponse).GetGenericArguments()[0];
                    var method = typeof(Result<>).MakeGenericType(resultType).GetMethod("ValidationFailure");
                    return (TResponse)method!.Invoke(null, new object[] { errorMessages })!;
                }
            }
            else if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.ValidationFailure(errorMessages);
            }

            // Fallback to throwing validation exception
            throw new ValidationException(failures);
        }

        return await next();
    }
}


