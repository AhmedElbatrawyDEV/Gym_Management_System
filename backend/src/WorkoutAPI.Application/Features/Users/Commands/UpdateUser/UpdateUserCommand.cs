using FluentValidation;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Application.Features.Users.Commands.UpdateUser;
    public class UpdateUserCommand : BaseCommand
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? PhoneNumber { get; init; }
        public DateTime DateOfBirth { get; init; }
        public Gender Gender { get; init; }
        public Language PreferredLanguage { get; init; }
    }

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x).Custom((command, context) =>
        {
            if (command.Id == Guid.Empty)
                context.AddFailure($"UserId is requird and can't be {Guid.Empty}");
        });
        RuleFor(x => x.PreferredLanguage).IsInEnum();
        RuleFor(x => x.FirstName)
        .NotEmpty().WithMessage("First name is required")
        .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters");

        RuleFor(x => x.DateOfBirth)
            .Must(BeAtLeast13YearsOld).WithMessage("User must be at least 13 years old")
            .Must(BeNotInFuture).WithMessage("Date of birth cannot be in the future");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be in valid international format");



    }
    private bool BeAtLeast13YearsOld(DateTime dateOfBirth)
    {
        return DateTime.UtcNow.Date.AddYears(-13) >= dateOfBirth.Date;
    }

    private bool BeNotInFuture(DateTime dateOfBirth)
    {
        return dateOfBirth.Date < DateTime.UtcNow.Date;
    }
}
