using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetWorkoutPlans;

public class GetWorkoutPlansQueryHandler : IRequestHandler<GetWorkoutPlansQuery, Result<List<WorkoutPlanDto>>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IUserRepository _userRepository;
    private readonly IExerciseRepository _exerciseRepository;

    public GetWorkoutPlansQueryHandler(
        IWorkoutPlanRepository workoutPlanRepository,
        IUserRepository userRepository,
        IExerciseRepository exerciseRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
        _userRepository = userRepository;
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<List<WorkoutPlanDto>>> Handle(GetWorkoutPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _workoutPlanRepository.GetFilteredAsync(
            request.CreatedBy,
            request.Type,
            request.Difficulty,
            request.ActiveOnly,
            cancellationToken);

        var creatorIds = plans.Select(p => p.CreatedBy).Distinct().ToList();
        var creators = await _userRepository.GetByIdsAsync(creatorIds, cancellationToken);
        var creatorLookup = creators.ToDictionary(c => c.Id, c => c.PersonalInfo.FullName);

        var exerciseIds = plans.SelectMany(p => p.Exercises.Select(e => e.ExerciseId)).Distinct().ToList();
        var exercises = await _exerciseRepository.GetByIdsAsync(exerciseIds, cancellationToken);
        var exerciseLookup = exercises.ToDictionary(e => e.Id, e => e.Code);

        var result = plans.Select(plan => new WorkoutPlanDto
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            Type = plan.Type,
            DifficultyLevel = plan.DifficultyLevel,
            DurationWeeks = plan.DurationWeeks,
            CreatedBy = plan.CreatedBy,
            CreatedByName = creatorLookup.GetValueOrDefault(plan.CreatedBy, string.Empty),
            IsActive = plan.IsActive,
            CreatedAt = plan.CreatedAt,
            Exercises = plan.Exercises.Select(e => new WorkoutPlanExerciseDto
            {
                ExerciseId = e.ExerciseId,
                ExerciseName = exerciseLookup.GetValueOrDefault(e.ExerciseId, string.Empty),
                Day = e.Day,
                Order = e.Order,
                Sets = e.Sets,
                Reps = e.Reps,
                Weight = e.Weight,
                Duration = e.Duration,
                RestTime = e.RestTime,
                Notes = e.Notes
            }).ToList()
        }).ToList();

        return Result<List<WorkoutPlanDto>>.Success(result);
    }
}