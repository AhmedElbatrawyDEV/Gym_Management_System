using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Queries.GetUserWorkoutSessions;
public class GetUserWorkoutSessionsQuery : BaseQuery<List<WorkoutSessionDto>>
{
    public Guid UserId { get; init; }
}