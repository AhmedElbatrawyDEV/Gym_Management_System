using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetUsersQuery;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedResult<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _userRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.Status,
            cancellationToken);

        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            FirstName = user.PersonalInfo.FirstName,
            LastName = user.PersonalInfo.LastName,
            FullName = user.PersonalInfo.FullName,
            Email = user.ContactInfo.Email,
            PhoneNumber = user.ContactInfo.PhoneNumber,
            DateOfBirth = user.PersonalInfo.DateOfBirth,
            Gender = user.PersonalInfo.Gender,
            Age = user.PersonalInfo.Age,
            ProfileImageUrl = user.ProfileImageUrl,
            Status = user.Status,
            MembershipNumber = user.MembershipNumber,
            PreferredLanguage = user.PreferredLanguage,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            HasActiveSubscription = user.HasActiveSubscription()
        }).ToList().AsReadOnly();

        return PaginatedResult<UserDto>.Success(userDtos, request.PageNumber, request.PageSize, totalCount);
    }
}