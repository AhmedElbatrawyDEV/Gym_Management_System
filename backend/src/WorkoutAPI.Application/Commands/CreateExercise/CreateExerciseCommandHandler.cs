using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CreateExercise;

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Result<Guid>>
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateExerciseCommandHandler(
        IExerciseRepository exerciseRepository,
        IUnitOfWork unitOfWork)
    {
        _exerciseRepository = exerciseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        // Check if code is unique
        var existingExercise = await _exerciseRepository.GetByCodeAsync(request.Code, cancellationToken) ?? throw new InvalidOperationException("Exercise with this code already exists");

        var exercise = Exercise.CreateNew(
            request.Code,
            request.Type,
            request.PrimaryMuscleGroup,
            request.Difficulty,
            request.SecondaryMuscleGroup,
            request.IconName);

        exercise.AddTranslation(request.Language, request.Name, request.Description, request.Instructions);

        await _unitOfWork.BeginAsync();
        try
        {
            await _exerciseRepository.AddAsync(exercise, cancellationToken);
            await _unitOfWork.Commit();
            return Result<Guid>.Success(exercise.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}
