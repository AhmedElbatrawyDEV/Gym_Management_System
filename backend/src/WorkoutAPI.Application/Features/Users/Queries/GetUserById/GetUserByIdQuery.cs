using FluentValidation;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.Features.Users.Commands.DeactivateUser;
using WorkoutAPI.Application.Features.Users.DTOs;

namespace WorkoutAPI.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : BaseQuery<UserDto>
{
    public Guid Id { get; init; }
}
public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x).Custom((command, context) =>
        {
            if (command.Id == Guid.Empty)
                context.AddFailure($"Id is requird and can't be {Guid.Empty}");
        });
    }
}