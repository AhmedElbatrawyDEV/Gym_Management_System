using FluentValidation;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Commands.CreateWorkoutPlan;
public class CreateWorkoutPlanCommand : BaseCommand<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public WorkoutPlanType Type { get; init; }
    public DifficultyLevel DifficultyLevel { get; init; }
    public int DurationWeeks { get; init; }
    public Guid CreatedBy { get; init; }
    public List<WorkoutPlanExerciseItem> Exercises { get; init; } = new();
}

public class WorkoutPlanExerciseItem
{
    public Guid ExerciseId { get; init; }
    public int Day { get; init; }
    public int Order { get; init; }
    public int Sets { get; init; }
    public int? Reps { get; init; }
    public decimal? Weight { get; init; }
    public TimeSpan? Duration { get; init; }
    public TimeSpan? RestTime { get; init; }
    public string? Notes { get; init; }
}
public class CreateWorkoutPlanCommandValidator : AbstractValidator<CreateWorkoutPlanCommand>
{
    public CreateWorkoutPlanCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.DurationWeeks)
            .GreaterThan(0).WithMessage("Duration must be at least 1 week");

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("Creator ID is required");

        RuleForEach(x => x.Exercises)
            .ChildRules(exercise =>
            {
                exercise.RuleFor(e => e.ExerciseId)
                    .NotEmpty().WithMessage("Exercise ID is required");

                exercise.RuleFor(e => e.Day)
                    .GreaterThan(0).WithMessage("Day must be at least 1");

                exercise.RuleFor(e => e.Order)
                    .GreaterThan(0).WithMessage("Order must be at least 1");

                exercise.RuleFor(e => e.Sets)
                    .GreaterThan(0).WithMessage("Sets must be at least 1");
            });
    }
}
