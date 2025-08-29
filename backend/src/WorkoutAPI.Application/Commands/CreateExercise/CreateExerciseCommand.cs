using FluentValidation;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Commands.CreateExercise;
public class CreateExerciseCommand : BaseCommand<Guid>
{
    public string Code { get; init; } = string.Empty;
    public ExerciseType Type { get; init; }
    public MuscleGroup PrimaryMuscleGroup { get; init; }
    public MuscleGroup? SecondaryMuscleGroup { get; init; }
    public DifficultyLevel Difficulty { get; init; }
    public string? IconName { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Instructions { get; init; }
    public Language Language { get; init; } = Language.English;
}
public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(20).WithMessage("Code must not exceed 20 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid exercise type");

        RuleFor(x => x.PrimaryMuscleGroup)
            .IsInEnum().WithMessage("Invalid muscle group");

        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Invalid difficulty level");
    }
}