using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetUserWorkoutSessions;

public class GetUserWorkoutSessionsQueryHandler : IRequestHandler<GetUserWorkoutSessionsQuery, Result<List<WorkoutSessionDto>>>
{
    private readonly IWorkoutSessionRepository _workoutSessionRepository;
    private readonly IUserRepository _userRepository;

    public GetUserWorkoutSessionsQueryHandler(
        IWorkoutSessionRepository workoutSessionRepository,
        IUserRepository userRepository)
    {
        _workoutSessionRepository = workoutSessionRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<List<WorkoutSessionDto>>> Handle(GetUserWorkoutSessionsQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _workoutSessionRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var trainerIds = sessions.Where(s => s.TrainerId.HasValue)
                                .Select(s => s.TrainerId.Value)
                                .Distinct()
                                .ToList();

        var trainers = trainerIds.Any()
            ? await _userRepository.GetByIdsAsync(trainerIds, cancellationToken)
            : new List<Domain.Aggregates.User>();

        var trainerLookup = trainers.ToDictionary(t => t.Id, t => t.PersonalInfo.FullName);

        var result = sessions.Select(session => new WorkoutSessionDto
        {
            Id = session.Id,
            UserId = session.UserId,
            TrainerId = session.TrainerId,
            TrainerName = session.TrainerId.HasValue
                ? trainerLookup.GetValueOrDefault(session.TrainerId.Value, string.Empty)
                : null,
            Title = session.Title,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            Duration = session.Duration,
            Status = session.Status,
            Notes = session.Notes
        }).ToList();

        return Result<List<WorkoutSessionDto>>.Success(result);
    }
}
