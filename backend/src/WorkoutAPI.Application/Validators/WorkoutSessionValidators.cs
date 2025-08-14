using FluentValidation;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Validators;

public class StartWorkoutSessionRequestValidator : AbstractValidator<StartWorkoutSessionRequest>
{
    public StartWorkoutSessionRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.WorkoutPlanId)
            .NotEmpty().WithMessage("Workout plan ID is required");
    }
}

public class CompleteExerciseRequestValidator : AbstractValidator<CompleteExerciseRequest>
{
    public CompleteExerciseRequestValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage("Exercise ID is required");

        RuleFor(x => x.Sets)
            .NotEmpty().WithMessage("At least one set is required")
            .Must(sets => sets.Count <= 10).WithMessage("Maximum 10 sets allowed per exercise");

        RuleForEach(x => x.Sets).SetValidator(new ExerciseSetRequestValidator());
    }
}

public class ExerciseSetRequestValidator : AbstractValidator<ExerciseSetRequest>
{
    public ExerciseSetRequestValidator()
    {
        RuleFor(x => x.SetNumber)
            .GreaterThan(0).WithMessage("Set number must be greater than 0")
            .LessThanOrEqualTo(10).WithMessage("Set number cannot exceed 10");

        RuleFor(x => x.Reps)
            .GreaterThan(0).WithMessage("Reps must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Reps cannot exceed 100");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0).WithMessage("Weight cannot be negative")
            .LessThan(1000).WithMessage("Weight cannot exceed 1000 kg");

        RuleFor(x => x.RestTime)
            .GreaterThanOrEqualTo(TimeSpan.Zero).WithMessage("Rest time cannot be negative")
            .LessThan(TimeSpan.FromMinutes(30)).WithMessage("Rest time cannot exceed 30 minutes");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

public class CompleteWorkoutSessionRequestValidator : AbstractValidator<CompleteWorkoutSessionRequest>
{
    public CompleteWorkoutSessionRequestValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

