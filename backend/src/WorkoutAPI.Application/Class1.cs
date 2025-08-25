using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkoutAPI.Application.Exercises.Commands.WorkoutAPI.Application.Exercises.Queries.WorkoutAPI.Application.Payments.Commands.WorkoutAPI.Application.Payments.Queries.WorkoutAPI.Application.Exercises.Handlers.WorkoutAPI.Application.Payments.Handlers.WorkoutAPI.Application.SubscriptionPlans.Queries.WorkoutAPI.Application.SubscriptionPlans.Handlers.WorkoutAPI.Application.Common.WorkoutAPI.Application.Common.Mappings.WorkoutAPI.Application.Common.Models.WorkoutAPI.Application.Users.Commands.WorkoutAPI.Application.Users.Queries.WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.Exercises.Commands.WorkoutAPI.Application.Exercises.Queries.WorkoutAPI.Application.Payments.Commands.WorkoutAPI.Application.Payments.Queries.WorkoutAPI.Application.Exercises.Handlers.WorkoutAPI.Application.Payments.Handlers.WorkoutAPI.Application.SubscriptionPlans.Queries.WorkoutAPI.Application.SubscriptionPlans.Handlers.WorkoutAPI.Application.Common.WorkoutAPI.Application.Common.Mappings.WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.Exercises.Commands.WorkoutAPI.Application.Exercises.Queries.WorkoutAPI.Application.Payments.Commands.WorkoutAPI.Application.Payments.Queries.WorkoutAPI.Application.Exercises.Handlers.WorkoutAPI.Application.Payments.Handlers.WorkoutAPI.Application.SubscriptionPlans.Queries.WorkoutAPI.Application.SubscriptionPlans.Handlers.WorkoutAPI.Application.Common;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Application;

}

// ===== EXERCISE COMMANDS & QUERIES =====

namespace WorkoutAPI.Application.Exercises.Commands;

public record CreateExerciseCommand(
    string Code,
    ExerciseType Type,
    MuscleGroup PrimaryMuscleGroup,
    DifficultyLevel Difficulty,
    MuscleGroup? SecondaryMuscleGroup = null,
    string? IconName = null
) : Command<Guid>;

public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid exercise type");

        RuleFor(x => x.PrimaryMuscleGroup)
            .IsInEnum().WithMessage("Invalid primary muscle group");

        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Invalid difficulty level");

        RuleFor(x => x.SecondaryMuscleGroup)
            .IsInEnum().WithMessage("Invalid secondary muscle group")
            .When(x => x.SecondaryMuscleGroup.HasValue);

        RuleFor(x => x.IconName)
            .MaximumLength(100).WithMessage("Icon name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.IconName));
    }
}

public record AddExerciseTranslationCommand(
    Guid ExerciseId,
    Language Language,
    string Name,
    string? Description = null,
    string? Instructions = null
) : Command;

public class AddExerciseTranslationCommandValidator : AbstractValidator<AddExerciseTranslationCommand>
{
    public AddExerciseTranslationCommandValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage("Exercise ID is required");

        RuleFor(x => x.Language)
            .IsInEnum().WithMessage("Invalid language");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Instructions)
            .MaximumLength(2000).WithMessage("Instructions cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Instructions));
    }
}

namespace WorkoutAPI.Application.Exercises.Queries;

public record GetExerciseByIdQuery(Guid Id) : Query<ExerciseResponse>;

public record GetExerciseByCodeQuery(string Code) : Query<ExerciseResponse>;

public record GetExercisesQuery(
    int PageNumber = 1,
    int PageSize = 20,
    ExerciseType? Type = null,
    MuscleGroup? MuscleGroup = null,
    DifficultyLevel? Difficulty = null,
    string? SearchTerm = null,
    Language Language = Language.English
) : Query<PagedResult<ExerciseResponse>>;

public class GetExercisesQueryValidator : AbstractValidator<GetExercisesQuery>
{
    public GetExercisesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");
    }
}

// ===== PAYMENT COMMANDS & QUERIES =====

namespace WorkoutAPI.Application.Payments.Commands;

public record CreatePaymentCommand(
    Guid UserId,
    decimal Amount,
    string Currency,
    PaymentMethod PaymentMethod,
    string? Description = null,
    Guid? UserSubscriptionId = null
) : Command<Guid>;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be 3 characters");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

public record ProcessPaymentCommand(
    Guid Id,
    string TransactionId
) : Command;

public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Payment ID is required");

        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("Transaction ID is required")
            .MaximumLength(100).WithMessage("Transaction ID cannot exceed 100 characters");
    }
}

public record RefundPaymentCommand(
    Guid Id,
    string Reason
) : Command;

public class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Payment ID is required");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Refund reason is required")
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters");
    }
}

namespace WorkoutAPI.Application.Payments.Queries;

public record GetPaymentByIdQuery(Guid Id) : Query<PaymentResponse>;

public record GetPaymentsByUserIdQuery(
    Guid UserId,
    int PageNumber = 1,
    int PageSize = 20
) : Query<PagedResult<PaymentResponse>>;

public record GetPaymentsByStatusQuery(
    PaymentStatus Status,
    int PageNumber = 1,
    int PageSize = 20
) : Query<PagedResult<PaymentResponse>>;

public record GetRevenueQuery(
    DateTime? StartDate = null,
    DateTime? EndDate = null
) : Query<decimal>;

// ===== EXERCISE HANDLERS =====

namespace WorkoutAPI.Application.Exercises.Handlers;

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Result<Guid>>
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateExerciseCommandHandler(IExerciseRepository exerciseRepository, IUnitOfWork unitOfWork)
    {
        _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            // Check if code is unique
            var existingExercise = await _exerciseRepository.GetByCodeAsync(request.Code, cancellationToken);
            if (existingExercise != null)
            {
                return Result.Failure<Guid>("Exercise code already exists");
            }

            // Create exercise
            var exercise = Exercise.CreateNew(
                request.Code,
                request.Type,
                request.PrimaryMuscleGroup,
                request.Difficulty,
                request.SecondaryMuscleGroup,
                request.IconName);

            // Save exercise
            await _exerciseRepository.AddAsync(exercise, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success(exercise.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure<Guid>($"Error creating exercise: {ex.Message}");
        }
    }
}

public class AddExerciseTranslationCommandHandler : IRequestHandler<AddExerciseTranslationCommand, Result>
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddExerciseTranslationCommandHandler(IExerciseRepository exerciseRepository, IUnitOfWork unitOfWork)
    {
        _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(AddExerciseTranslationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            var exercise = await _exerciseRepository.GetByIdAsync(request.ExerciseId, cancellationToken);
            if (exercise == null)
            {
                return Result.Failure("Exercise not found");
            }

            exercise.AddTranslation(request.Language, request.Name, request.Description, request.Instructions);

            await _exerciseRepository.UpdateAsync(exercise, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure($"Error adding translation: {ex.Message}");
        }
    }
}

public class GetExerciseByIdQueryHandler : IRequestHandler<GetExerciseByIdQuery, Result<ExerciseResponse>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExerciseByIdQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
    }

    public async Task<Result<ExerciseResponse>> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var exercise = await _exerciseRepository.GetByIdAsync(request.Id, cancellationToken);
            if (exercise == null)
            {
                return Result.Failure<ExerciseResponse>("Exercise not found");
            }

            var response = exercise.Adapt<ExerciseResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<ExerciseResponse>($"Error retrieving exercise: {ex.Message}");
        }
    }
}

public class GetExerciseByCodeQueryHandler : IRequestHandler<GetExerciseByCodeQuery, Result<ExerciseResponse>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExerciseByCodeQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
    }

    public async Task<Result<ExerciseResponse>> Handle(GetExerciseByCodeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var exercise = await _exerciseRepository.GetByCodeAsync(request.Code, cancellationToken);
            if (exercise == null)
            {
                return Result.Failure<ExerciseResponse>("Exercise not found");
            }

            var response = exercise.Adapt<ExerciseResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<ExerciseResponse>($"Error retrieving exercise: {ex.Message}");
        }
    }
}

public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, Result<PagedResult<ExerciseResponse>>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExercisesQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository ?? throw new ArgumentNullException(nameof(exerciseRepository));
    }

    public async Task<Result<PagedResult<ExerciseResponse>>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Exercise> exercises;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                exercises = await _exerciseRepository.SearchAsync(request.SearchTerm, request.Language, cancellationToken);
            }
            else if (request.Type.HasValue)
            {
                exercises = await _exerciseRepository.GetByTypeAsync(request.Type.Value, cancellationToken);
            }
            else if (request.MuscleGroup.HasValue)
            {
                exercises = await _exerciseRepository.GetByMuscleGroupAsync(request.MuscleGroup.Value, cancellationToken);
            }
            else if (request.Difficulty.HasValue)
            {
                exercises = await _exerciseRepository.GetByDifficultyAsync(request.Difficulty.Value, cancellationToken);
            }
            else
            {
                exercises = await _exerciseRepository.GetActiveExercisesAsync(cancellationToken);
            }

            var totalCount = exercises.Count();
            var pagedExercises = exercises
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var responses = pagedExercises.Adapt<List<ExerciseResponse>>();

            var pagedResult = new PagedResult<ExerciseResponse>
            {
                Items = responses,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result.Failure<PagedResult<ExerciseResponse>>($"Error retrieving exercises: {ex.Message}");
        }
    }
}

// ===== PAYMENT HANDLERS =====

namespace WorkoutAPI.Application.Payments.Handlers;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Guid>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            // Validate user exists
            var userExists = await _userRepository.ExistsAsync(request.UserId, cancellationToken);
            if (!userExists)
            {
                return Result.Failure<Guid>("User not found");
            }

            // Create money value object
            var money = new Money(request.Amount, request.Currency);

            // Create payment
            var payment = Payment.CreateNew(
                request.UserId,
                money,
                request.PaymentMethod,
                request.Description,
                request.UserSubscriptionId);

            // Save payment
            await _paymentRepository.AddAsync(payment, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success(payment.Guid);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure<Guid>($"Error creating payment: {ex.Message}");
        }
    }
}

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessPaymentCommandHandler(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            var payment = await _paymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (payment == null)
            {
                return Result.Failure("Payment not found");
            }

            payment.ProcessPayment(request.TransactionId);

            await _paymentRepository.UpdateAsync(payment, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure($"Error processing payment: {ex.Message}");
        }
    }
}

public class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefundPaymentCommandHandler(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            var payment = await _paymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (payment == null)
            {
                return Result.Failure("Payment not found");
            }

            payment.RefundPayment(request.Reason);

            await _paymentRepository.UpdateAsync(payment, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure($"Error refunding payment: {ex.Message}");
        }
    }
}

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Result<PaymentResponse>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    public async Task<Result<PaymentResponse>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _paymentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (payment == null)
            {
                return Result.Failure<PaymentResponse>("Payment not found");
            }

            var response = payment.Adapt<PaymentResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaymentResponse>($"Error retrieving payment: {ex.Message}");
        }
    }
}

public class GetPaymentsByUserIdQueryHandler : IRequestHandler<GetPaymentsByUserIdQuery, Result<PagedResult<PaymentResponse>>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentsByUserIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    public async Task<Result<PagedResult<PaymentResponse>>> Handle(GetPaymentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var payments = await _paymentRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            var totalCount = payments.Count();
            var pagedPayments = payments
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var responses = pagedPayments.Adapt<List<PaymentResponse>>();

            var pagedResult = new PagedResult<PaymentResponse>
            {
                Items = responses,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result.Failure<PagedResult<PaymentResponse>>($"Error retrieving payments: {ex.Message}");
        }
    }
}

public class GetPaymentsByStatusQueryHandler : IRequestHandler<GetPaymentsByStatusQuery, Result<PagedResult<PaymentResponse>>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentsByStatusQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    public async Task<Result<PagedResult<PaymentResponse>>> Handle(GetPaymentsByStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var payments = await _paymentRepository.GetByStatusAsync(request.Status, cancellationToken);

            var totalCount = payments.Count();
            var pagedPayments = payments
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var responses = pagedPayments.Adapt<List<PaymentResponse>>();

            var pagedResult = new PagedResult<PaymentResponse>
            {
                Items = responses,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result.Failure<PagedResult<PaymentResponse>>($"Error retrieving payments: {ex.Message}");
        }
    }
}

public class GetRevenueQueryHandler : IRequestHandler<GetRevenueQuery, Result<decimal>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetRevenueQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
    }

    public async Task<Result<decimal>> Handle(GetRevenueQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var totalRevenue = await _paymentRepository.GetTotalRevenueAsync(
                request.StartDate,
                request.EndDate,
                cancellationToken);

            return Result.Success(totalRevenue);
        }
        catch (Exception ex)
        {
            return Result.Failure<decimal>($"Error calculating revenue: {ex.Message}");
        }
    }
}

// ===== SUBSCRIPTION PLAN QUERIES =====

namespace WorkoutAPI.Application.SubscriptionPlans.Queries;

public record GetSubscriptionPlanByIdQuery(Guid Id) : Query<SubscriptionPlanResponse>;

public record GetActiveSubscriptionPlansQuery(
    int PageNumber = 1,
    int PageSize = 20
) : Query<PagedResult<SubscriptionPlanResponse>>;

public record GetSubscriptionPlansByPriceRangeQuery(
    decimal MinPrice,
    decimal MaxPrice,
    int PageNumber = 1,
    int PageSize = 20
) : Query<PagedResult<SubscriptionPlanResponse>>;

public class GetSubscriptionPlansByPriceRangeQueryValidator : AbstractValidator<GetSubscriptionPlansByPriceRangeQuery>
{
    public GetSubscriptionPlansByPriceRangeQueryValidator()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum price cannot be negative");

        RuleFor(x => x.MaxPrice)
            .GreaterThan(x => x.MinPrice).WithMessage("Maximum price must be greater than minimum price");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50");
    }
}

// ===== SUBSCRIPTION PLAN HANDLERS =====

namespace WorkoutAPI.Application.SubscriptionPlans.Handlers;

public class GetSubscriptionPlanByIdQueryHandler : IRequestHandler<GetSubscriptionPlanByIdQuery, Result<SubscriptionPlanResponse>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public GetSubscriptionPlanByIdQueryHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository ?? throw new ArgumentNullException(nameof(subscriptionPlanRepository));
    }

    public async Task<Result<SubscriptionPlanResponse>> Handle(GetSubscriptionPlanByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var subscriptionPlan = await _subscriptionPlanRepository.GetByIdAsync(request.Id, cancellationToken);
            if (subscriptionPlan == null)
            {
                return Result.Failure<SubscriptionPlanResponse>("Subscription plan not found");
            }

            var response = subscriptionPlan.Adapt<SubscriptionPlanResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<SubscriptionPlanResponse>($"Error retrieving subscription plan: {ex.Message}");
        }
    }
}

public class GetActiveSubscriptionPlansQueryHandler : IRequestHandler<GetActiveSubscriptionPlansQuery, Result<PagedResult<SubscriptionPlanResponse>>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public GetActiveSubscriptionPlansQueryHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository ?? throw new ArgumentNullException(nameof(subscriptionPlanRepository));
    }

    public async Task<Result<PagedResult<SubscriptionPlanResponse>>> Handle(GetActiveSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var subscriptionPlans = await _subscriptionPlanRepository.GetActiveAsync(cancellationToken);

            var totalCount = subscriptionPlans.Count();
            var pagedPlans = subscriptionPlans
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var responses = pagedPlans.Adapt<List<SubscriptionPlanResponse>>();

            var pagedResult = new PagedResult<SubscriptionPlanResponse>
            {
                Items = responses,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result.Failure<PagedResult<SubscriptionPlanResponse>>($"Error retrieving subscription plans: {ex.Message}");
        }
    }
}

public class GetSubscriptionPlansByPriceRangeQueryHandler : IRequestHandler<GetSubscriptionPlansByPriceRangeQuery, Result<PagedResult<SubscriptionPlanResponse>>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public GetSubscriptionPlansByPriceRangeQueryHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository ?? throw new ArgumentNullException(nameof(subscriptionPlanRepository));
    }

    public async Task<Result<PagedResult<SubscriptionPlanResponse>>> Handle(GetSubscriptionPlansByPriceRangeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var subscriptionPlans = await _subscriptionPlanRepository.GetByPriceRangeAsync(
                request.MinPrice,
                request.MaxPrice,
                cancellationToken);

            var totalCount = subscriptionPlans.Count();
            var pagedPlans = subscriptionPlans
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var responses = pagedPlans.Adapt<List<SubscriptionPlanResponse>>();

            var pagedResult = new PagedResult<SubscriptionPlanResponse>
            {
                Items = responses,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result.Failure<PagedResult<SubscriptionPlanResponse>>($"Error retrieving subscription plans: {ex.Message}");
        }
    }
}// ===== APPLICATION LAYER STRUCTURE =====

// ===== COMMON TYPES =====

using MediatR;

namespace WorkoutAPI.Application.Common;

public abstract record Command : IRequest<Result>;
public abstract record Command<TResponse> : IRequest<Result<TResponse>>;
public abstract record Query<TResponse> : IRequest<Result<TResponse>>;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && error != string.Empty)
            throw new InvalidOperationException();
        if (!isSuccess && error == string.Empty)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);
    public static Result<T> Success<T>(T value) => new(value, true, string.Empty);
    public static Result<T> Failure<T>(string error) => new(default, false, error);
}

public class Result<T> : Result
{
    private readonly T? _value;

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access value of failed result");
            return _value!;
        }
    }

    protected internal Result(T? value, bool isSuccess, string error)
        : base(isSuccess, error)
    {
        _value = value;
    }
}

// ===== MAPPING PROFILES =====

using Mapster;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.ValueObjects;
using WorkoutAPI.Application.Users.Commands;
using WorkoutAPI.Application.Users.Queries;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Common.Mappings;

public class MappingConfig
{
    public static void Configure()
    {
        // User mappings
        TypeAdapterConfig<User, UserResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Guid)
            .Map(dest => dest.FirstName, src => src.PersonalInfo.FirstName)
            .Map(dest => dest.LastName, src => src.PersonalInfo.LastName)
            .Map(dest => dest.FullName, src => src.PersonalInfo.FullName)
            .Map(dest => dest.Email, src => src.ContactInfo.Email)
            .Map(dest => dest.PhoneNumber, src => src.ContactInfo.PhoneNumber)
            .Map(dest => dest.DateOfBirth, src => src.PersonalInfo.DateOfBirth)
            .Map(dest => dest.Gender, src => src.PersonalInfo.Gender)
            .Map(dest => dest.Age, src => src.PersonalInfo.Age);

        // Exercise mappings
        TypeAdapterConfig<Exercise, ExerciseResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Type, src => src.Type.ToString())
            .Map(dest => dest.PrimaryMuscleGroup, src => src.PrimaryMuscleGroup.ToString())
            .Map(dest => dest.SecondaryMuscleGroup, src => src.SecondaryMuscleGroup.ToString())
            .Map(dest => dest.Difficulty, src => src.Difficulty.ToString())
            .Map(dest => dest.IconName, src => src.IconName)
            .Map(dest => dest.IsActive, src => src.IsActive);

        // WorkoutSession mappings
        TypeAdapterConfig<WorkoutSession, WorkoutSessionResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Guid)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.TrainerId, src => src.TrainerId)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.StartTime, src => src.StartTime)
            .Map(dest => dest.EndTime, src => src.EndTime)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.Notes, src => src.Notes)
            .Map(dest => dest.Duration, src => src.Duration);

        // Payment mappings
        TypeAdapterConfig<Payment, PaymentResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Guid)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Amount, src => src.Amount.Amount)
            .Map(dest => dest.Currency, src => src.Amount.Currency)
            .Map(dest => dest.PaymentMethod, src => src.PaymentMethod.ToString())
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.PaymentDate, src => src.PaymentDate)
            .Map(dest => dest.TransactionId, src => src.TransactionId)
            .Map(dest => dest.Description, src => src.Description);

        // SubscriptionPlan mappings
        TypeAdapterConfig<SubscriptionPlan, SubscriptionPlanResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Price, src => src.Price.Amount)
            .Map(dest => dest.Currency, src => src.Price.Currency)
            .Map(dest => dest.DurationDays, src => src.DurationDays)
            .Map(dest => dest.Features, src => src.Features)
            .Map(dest => dest.IsActive, src => src.IsActive);
    }
}

// ===== RESPONSE MODELS =====

namespace WorkoutAPI.Application.Common.Models;

public record UserResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string Gender { get; init; } = string.Empty;
    public int Age { get; init; }
    public string? ProfileImageUrl { get; init; }
    public string Status { get; init; } = string.Empty;
    public string MembershipNumber { get; init; } = string.Empty;
    public string PreferredLanguage { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public record ExerciseResponse
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string PrimaryMuscleGroup { get; init; } = string.Empty;
    public string? SecondaryMuscleGroup { get; init; }
    public string Difficulty { get; init; } = string.Empty;
    public string? IconName { get; init; }
    public bool IsActive { get; init; }
    public List<ExerciseTranslationResponse> Translations { get; init; } = new();
}

public record ExerciseTranslationResponse
{
    public string Language { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Instructions { get; init; }
}

public record WorkoutSessionResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid? TrainerId { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public TimeSpan? Duration { get; init; }
    public List<WorkoutSessionExerciseResponse> Exercises { get; init; } = new();
}

public record WorkoutSessionExerciseResponse
{
    public Guid Id { get; init; }
    public Guid ExerciseId { get; init; }
    public int Order { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime? StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public string? Notes { get; init; }
    public List<ExerciseSetResponse> Sets { get; init; } = new();
}

public record ExerciseSetResponse
{
    public int SetNumber { get; init; }
    public int? Reps { get; init; }
    public decimal? Weight { get; init; }
    public TimeSpan? Duration { get; init; }
    public int? Distance { get; init; }
}

public record PaymentResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string PaymentMethod { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime? PaymentDate { get; init; }
    public string? TransactionId { get; init; }
    public string? Description { get; init; }
}

public record SubscriptionPlanResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = string.Empty;
    public int DurationDays { get; init; }
    public List<string> Features { get; init; } = new();
    public bool IsActive { get; init; }
}

// ===== USER COMMANDS =====

using FluentValidation;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Users.Commands;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    DateTime DateOfBirth,
    Gender Gender,
    Language PreferredLanguage
) : Command<Guid>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Invalid date of birth");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender value");

        RuleFor(x => x.PreferredLanguage)
            .IsInEnum().WithMessage("Invalid language value");
    }
}

public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    DateTime DateOfBirth,
    Gender Gender,
    Language PreferredLanguage
) : Command;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Invalid date of birth");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender value");

        RuleFor(x => x.PreferredLanguage)
            .IsInEnum().WithMessage("Invalid language value");
    }
}

public record DeleteUserCommand(Guid Id) : Command;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required");
    }
}

public record SubscribeUserCommand(
    Guid UserId,
    Guid SubscriptionPlanId,
    DateTime StartDate
) : Command<Guid>;

public class SubscribeUserCommandValidator : AbstractValidator<SubscribeUserCommand>
{
    public SubscribeUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.SubscriptionPlanId)
            .NotEmpty().WithMessage("Subscription plan ID is required");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the past");
    }
}

// ===== USER QUERIES =====

namespace WorkoutAPI.Application.Users.Queries;

public record GetUserByIdQuery(Guid Id) : Query<UserResponse>;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required");
    }
}

public record GetUserByEmailQuery(string Email) : Query<UserResponse>;

public class GetUserByEmailQueryValidator : AbstractValidator<GetUserByEmailQuery>
{
    public GetUserByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}

public record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    UserStatus? Status = null
) : Query<PagedResult<UserResponse>>;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");
    }
}

// ===== PAGED RESULT =====

namespace WorkoutAPI.Application.Common.Models;

public record PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

// ===== USER HANDLERS =====

using MediatR;
using Mapster;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Repositories;
using WorkoutAPI.Domain.ValueObjects;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Application.Common;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.Users.Commands;
using WorkoutAPI.Application.Users.Queries;

namespace WorkoutAPI.Application.Users.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            // Check if email is unique
            var isEmailUnique = await _userRepository.IsEmailUniqueAsync(request.Email, cancellationToken: cancellationToken);
            if (!isEmailUnique)
            {
                return Result.Failure<Guid>("Email already exists");
            }

            // Create value objects
            var personalInfo = new PersonalInfo(
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Gender);

            var contactInfo = new ContactInfo(request.Email, request.PhoneNumber);

            // Create user aggregate
            var user = User.CreateNew(personalInfo, contactInfo, request.PreferredLanguage);

            // Save user
            await _userRepository.AddAsync(user, cancellationToken);

            // Commit transaction
            await _unitOfWork.Commit();

            return Result.Success(user.Guid);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure<Guid>($"Error creating user: {ex.Message}");
        }
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            // Get user
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            // Check if email is unique (excluding current user)
            var isEmailUnique = await _userRepository.IsEmailUniqueAsync(
                request.Email,
                request.Id,
                cancellationToken);
            if (!isEmailUnique)
            {
                return Result.Failure("Email already exists");
            }

            // Create value objects
            var personalInfo = new PersonalInfo(
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Gender);

            var contactInfo = new ContactInfo(request.Email, request.PhoneNumber);

            // Update user
            user.UpdatePersonalInfo(personalInfo);
            user.UpdateContactInfo(contactInfo);

            // Save changes
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure($"Error updating user: {ex.Message}");
        }
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            // Check if user exists
            var userExists = await _userRepository.ExistsAsync(request.Id, cancellationToken);
            if (!userExists)
            {
                return Result.Failure("User not found");
            }

            // Delete user
            await _userRepository.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure($"Error deleting user: {ex.Message}");
        }
    }
}

public class SubscribeUserCommandHandler : IRequestHandler<SubscribeUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubscribeUserCommandHandler(
        IUserRepository userRepository,
        ISubscriptionPlanRepository subscriptionPlanRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _subscriptionPlanRepository = subscriptionPlanRepository ?? throw new ArgumentNullException(nameof(subscriptionPlanRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(SubscribeUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            // Get user
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure<Guid>("User not found");
            }

            // Get subscription plan
            var subscriptionPlan = await _subscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanId, cancellationToken);
            if (subscriptionPlan == null)
            {
                return Result.Failure<Guid>("Subscription plan not found");
            }

            if (!subscriptionPlan.IsActive)
            {
                return Result.Failure<Guid>("Subscription plan is not active");
            }

            // Subscribe user
            user.SubscribeTo(subscriptionPlan, request.StartDate);

            // Save changes
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success(user.Guid);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure<Guid>($"Error subscribing user: {ex.Message}");
        }
    }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
            {
                return Result.Failure<UserResponse>("User not found");
            }

            var userResponse = user.Adapt<UserResponse>();
            return Result.Success(userResponse);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserResponse>($"Error retrieving user: {ex.Message}");
        }
    }
}

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
            {
                return Result.Failure<UserResponse>("User not found");
            }

            var userResponse = user.Adapt<UserResponse>();
            return Result.Success(userResponse);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserResponse>($"Error retrieving user: {ex.Message}");
        }
    }
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PagedResult<UserResponse>>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<PagedResult<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<User> users;

            if (request.Status.HasValue)
            {
                users = await _userRepository.GetUsersByStatusAsync(request.Status.Value, cancellationToken);
            }
            else if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                users = await _userRepository.SearchAsync(request.SearchTerm, cancellationToken);
            }
            else
            {
                users = await _userRepository.GetAllAsync(cancellationToken);
            }

            var totalCount = users.Count();
            var pagedUsers = users
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var userResponses = pagedUsers.Adapt<List<UserResponse>>();

            var pagedResult = new PagedResult<UserResponse>
            {
                Items = userResponses,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result.Success(pagedResult);
        }
        catch (Exception ex)
        {
            return Result.Failure<PagedResult<UserResponse>>($"Error retrieving users: {ex.Message}");
        }
    }
}

// ===== WORKOUT SESSION COMMANDS =====

namespace WorkoutAPI.Application.WorkoutSessions.Commands;

public record CreateWorkoutSessionCommand(
    Guid UserId,
    string Title,
    DateTime StartTime,
    Guid? TrainerId = null
) : Command<Guid>;

public class CreateWorkoutSessionCommandValidator : AbstractValidator<CreateWorkoutSessionCommand>
{
    public CreateWorkoutSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow.AddMinutes(-30))
            .WithMessage("Start time cannot be more than 30 minutes in the past");
    }
}

public record StartWorkoutSessionCommand(Guid Id) : Command;

public class StartWorkoutSessionCommandValidator : AbstractValidator<StartWorkoutSessionCommand>
{
    public StartWorkoutSessionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Workout session ID is required");
    }
}

public record CompleteWorkoutSessionCommand(
    Guid Id,
    string? Notes = null
) : Command;

public class CompleteWorkoutSessionCommandValidator : AbstractValidator<CompleteWorkoutSessionCommand>
{
    public CompleteWorkoutSessionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Workout session ID is required");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

public record AddExerciseToSessionCommand(
    Guid SessionId,
    Guid ExerciseId,
    int Order
) : Command;

public class AddExerciseToSessionCommandValidator : AbstractValidator<AddExerciseToSessionCommand>
{
    public AddExerciseToSessionCommandValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty().WithMessage("Session ID is required");

        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage("Exercise ID is required");

        RuleFor(x => x.Order)
            .GreaterThan(0).WithMessage("Order must be greater than 0");
    }
}

// ===== WORKOUT SESSION QUERIES =====

namespace WorkoutAPI.Application.WorkoutSessions.Queries;

public record GetWorkoutSessionByIdQuery(Guid Id) : Query<WorkoutSessionResponse>;

public class GetWorkoutSessionByIdQueryValidator : AbstractValidator<GetWorkoutSessionByIdQuery>
{
    public GetWorkoutSessionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Workout session ID is required");
    }
}

public record GetWorkoutSessionsByUserIdQuery(
    Guid UserId,
    int PageNumber = 1,
    int PageSize = 10
) : Query<PagedResult<WorkoutSessionResponse>>;

public class GetWorkoutSessionsByUserIdQueryValidator : AbstractValidator<GetWorkoutSessionsByUserIdQuery>
{
    public GetWorkoutSessionsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50");
    }
}

public record GetActiveWorkoutSessionsQuery(
    int PageNumber = 1,
    int PageSize = 10
) : Query<PagedResult<WorkoutSessionResponse>>;

// ===== WORKOUT SESSION HANDLERS =====

namespace WorkoutAPI.Application.WorkoutSessions.Handlers;

public class CreateWorkoutSessionCommandHandler : IRequestHandler<CreateWorkoutSessionCommand, Result<Guid>>
{
    private readonly IWorkoutSessionRepository _workoutSessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkoutSessionCommandHandler(
        IWorkoutSessionRepository workoutSessionRepository,
        IUserRepository userRepository,
        ITrainerRepository trainerRepository,
        IUnitOfWork unitOfWork)
    {
        _workoutSessionRepository = workoutSessionRepository ?? throw new ArgumentNullException(nameof(workoutSessionRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _trainerRepository = trainerRepository ?? throw new ArgumentNullException(nameof(trainerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(CreateWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            // Validate user exists
            var userExists = await _userRepository.ExistsAsync(request.UserId, cancellationToken);
            if (!userExists)
            {
                return Result.Failure<Guid>("User not found");
            }

            // Validate trainer exists if provided
            if (request.TrainerId.HasValue)
            {
                var trainerExists = await _trainerRepository.ExistsAsync(request.TrainerId.Value, cancellationToken);
                if (!trainerExists)
                {
                    return Result.Failure<Guid>("Trainer not found");
                }
            }

            // Create workout session
            var workoutSession = WorkoutSession.CreateNew(
                request.UserId,
                request.Title,
                request.StartTime,
                request.TrainerId);

            // Save workout session
            await _workoutSessionRepository.AddAsync(workoutSession, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success(workoutSession.Guid);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure<Guid>($"Error creating workout session: {ex.Message}");
        }
    }
}

public class StartWorkoutSessionCommandHandler : IRequestHandler<StartWorkoutSessionCommand, Result>
{
    private readonly IWorkoutSessionRepository _workoutSessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartWorkoutSessionCommandHandler(
        IWorkoutSessionRepository workoutSessionRepository,
        IUnitOfWork unitOfWork)
    {
        _workoutSessionRepository = workoutSessionRepository ?? throw new ArgumentNullException(nameof(workoutSessionRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(StartWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            var workoutSession = await _workoutSessionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (workoutSession == null)
            {
                return Result.Failure("Workout session not found");
            }

            workoutSession.StartSession();

            await _workoutSessionRepository.UpdateAsync(workoutSession, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure($"Error starting workout session: {ex.Message}");
        }
    }
}

public class CompleteWorkoutSessionCommandHandler : IRequestHandler<CompleteWorkoutSessionCommand, Result>
{
    private readonly IWorkoutSessionRepository _workoutSessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteWorkoutSessionCommandHandler(
        IWorkoutSessionRepository workoutSessionRepository,
        IUnitOfWork unitOfWork)
    {
        _workoutSessionRepository = workoutSessionRepository ?? throw new ArgumentNullException(nameof(workoutSessionRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(CompleteWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            var workoutSession = await _workoutSessionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (workoutSession == null)
            {
                return Result.Failure("Workout session not found");
            }

            workoutSession.CompleteSession(request.Notes);

            await _workoutSessionRepository.UpdateAsync(workoutSession, cancellationToken);
            await _unitOfWork.Commit();

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();
            return Result.Failure($"Error completing workout session: {ex.Message}");
        }
    }
}

public class GetWorkoutSessionByIdQueryHandler : IRequestHandler<GetWorkoutSessionByIdQuery, Result<WorkoutSessionResponse>>
{
    private readonly IWorkoutSessionRepository _workoutSessionRepository;

    public GetWorkoutSessionByIdQueryHandler(IWorkoutSessionRepository workoutSessionRepository)
    {
        _workoutSessionRepository = workoutSessionRepository ?? throw new ArgumentNullException(nameof(workoutSessionRepository));
    }

    public async Task<Result<WorkoutSessionResponse>> Handle(GetWorkoutSessionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var workoutSession = await _workoutSessionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (workoutSession == null)
            {
                return Result.Failure<WorkoutSessionResponse>("Workout session not found");
            }

            var response = workoutSession.Adapt<WorkoutSessionResponse>();
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<WorkoutSessionResponse>($"Error retrieving workout session: {ex.Message}");
        }
    }
}

// ===== APPLICATION SERVICE REGISTRATION =====

using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WorkoutAPI.Application.Common.Behaviors;
using WorkoutAPI.Application.Common.Mappings;

namespace WorkoutAPI.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        // Add FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Configure Mapster
        var config = TypeAdapterConfig.GlobalSettings;
        MappingConfig.Configure();
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}

// ===== PIPELINE BEHAVIORS =====

namespace WorkoutAPI.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));

                // Handle Result<T> types
                if (typeof(TResponse).IsGenericType &&
                    typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultType = typeof(TResponse).GetGenericArguments()[0];
                    var failureMethod = typeof(Result).GetMethod(nameof(Result.Failure))!
                        .MakeGenericMethod(resultType);

                    return (TResponse)failureMethod.Invoke(null, new object[] { errorMessage })!;
                }

                // Handle Result type
                if (typeof(TResponse) == typeof(Result))
                {
                    return (TResponse)(object)Result.Failure(errorMessage);
                }

                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {RequestName}", requestName);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var response = await next();

            stopwatch.Stop();
            _logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms",
                requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Error handling {RequestName} in {ElapsedMilliseconds}ms",
                requestName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}