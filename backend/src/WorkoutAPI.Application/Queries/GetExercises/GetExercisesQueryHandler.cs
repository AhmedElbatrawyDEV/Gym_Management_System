using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetExercises;

public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, PaginatedResult<ExerciseDto>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExercisesQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<PaginatedResult<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
    {
        var (exercises, totalCount) = await _exerciseRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.Type,
            request.MuscleGroup,
            request.Difficulty,
            request.ActiveOnly,
            cancellationToken);

        var exerciseDtos = exercises.Select(exercise =>
        {
            var translation = exercise.Translations.FirstOrDefault(t => t.Language == request.Language) ??
                             exercise.Translations.FirstOrDefault(t => t.Language == Language.English) ??
                             exercise.Translations.First();

            return new ExerciseDto
            {
                Id = exercise.Id,
                Code = exercise.Code,
                Name = translation.Name,
                Type = exercise.Type,
                PrimaryMuscleGroup = exercise.PrimaryMuscleGroup,
                SecondaryMuscleGroup = exercise.SecondaryMuscleGroup,
                Difficulty = exercise.Difficulty,
                IconName = exercise.IconName,
                IsActive = exercise.IsActive,
                Description = translation.Description,
                Instructions = translation.Instructions
            };
        }).ToList().AsReadOnly();

        return PaginatedResult<ExerciseDto>.Success(exerciseDtos, request.PageNumber, request.PageSize, totalCount);
    }
}