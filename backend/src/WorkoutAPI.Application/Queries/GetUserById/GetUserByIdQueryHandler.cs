using Mapster;
using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetUserById;


public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (userEntity == null)
            return Result<UserDto>.Failure($"User with ID {request.Id} not found.");

        var dto = userEntity.Adapt<UserDto>();
        return Result<UserDto>.Success(dto);
    }
}