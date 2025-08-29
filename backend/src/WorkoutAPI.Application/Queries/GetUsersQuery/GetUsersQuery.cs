using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Queries.GetUsersQuery;

public class GetUsersQuery : BasePaginatedQuery<UserDto>
{
    public string? SearchTerm { get; init; }
    public UserStatus? Status { get; init; }
}