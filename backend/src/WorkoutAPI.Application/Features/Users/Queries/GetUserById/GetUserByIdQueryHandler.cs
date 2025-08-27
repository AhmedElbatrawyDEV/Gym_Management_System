using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Interfaces;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.Features.Users.DTOs;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Features.Users.Queries.GetUserById;


public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(User), request.Id);
        var dto = _mapper.Map<UserDto>(userEntity);
   
        return Result<UserDto>.Success(dto);
    }
}
