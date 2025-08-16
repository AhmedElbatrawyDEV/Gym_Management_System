using FluentValidation;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Validators;
public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}