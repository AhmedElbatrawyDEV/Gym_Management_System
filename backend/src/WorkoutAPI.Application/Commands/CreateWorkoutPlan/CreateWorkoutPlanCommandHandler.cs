using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CreateWorkoutPlan;

public class CreateWorkoutPlanCommandHandler : IRequestHandler<CreateWorkoutPlanCommand, Result<Guid>>
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkoutPlanCommandHandler(
        IWorkoutPlanRepository workoutPlanRepository,
        IExerciseRepository exerciseRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _workoutPlanRepository = workoutPlanRepository;
        _exerciseRepository = exerciseRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        // Validate creator exists
        var creatorExists = await _userRepository.ExistsAsync(request.CreatedBy, cancellationToken);
        if (!creatorExists)
            throw new NotFoundException("User", request.CreatedBy);

        var workoutPlan = WorkoutPlan.CreateNew(
            request.Name,
            request.Type,
            request.DifficultyLevel,
            request.DurationWeeks,
            request.CreatedBy,
            request.Description);

        // Add exercises
        foreach (var exerciseItem in request.Exercises)
        {
            // Validate exercise exists
            var exerciseExists = await _exerciseRepository.GetByIdAsync(exerciseItem.ExerciseId, cancellationToken) ??
                throw new NotFoundException("Exercise", exerciseItem.ExerciseId);

            workoutPlan.AddExercise(
                exerciseExists,
                exerciseItem.Day,
                exerciseItem.Order,
                exerciseItem.Sets,
                exerciseItem.Reps,
                exerciseItem.Weight,
                exerciseItem.Duration,
                exerciseItem.RestTime,
                exerciseItem.Notes);
        }

        await _unitOfWork.BeginAsync();
        try
        {
            await _workoutPlanRepository.AddAsync(workoutPlan, cancellationToken);
            await _unitOfWork.Commit();
            return Result<Guid>.Success(workoutPlan.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}
