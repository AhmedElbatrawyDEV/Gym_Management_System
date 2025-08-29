using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CreateUserSubscription;

public class CreateUserSubscriptionCommandHandler : IRequestHandler<CreateUserSubscriptionCommand, Result<Guid>>
{
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserSubscriptionCommandHandler(
        IUserSubscriptionRepository userSubscriptionRepository,
        IUserRepository userRepository,
        ISubscriptionPlanRepository subscriptionPlanRepository,
        IUnitOfWork unitOfWork)
    {
        _userSubscriptionRepository = userSubscriptionRepository;
        _userRepository = userRepository;
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserSubscriptionCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken) ?? throw new NotFoundException("User", request.UserId);

        // Validate subscription plan exists and is active
        var subscriptionPlan = await _subscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanId, cancellationToken) ?? throw new NotFoundException("SubscriptionPlan", request.SubscriptionPlanId);

        if (!subscriptionPlan.IsActive)
            throw new InvalidOperationException("Subscription plan is not active");

        // Create subscription
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(subscriptionPlan.DurationDays);
        var period = new Domain.ValueObjects.DateRange(startDate, endDate);

        var userSubscription = UserSubscription.CreateNew(request.UserId, request.SubscriptionPlanId, period);

        await _unitOfWork.BeginAsync();
        try
        {
            await _userSubscriptionRepository.AddAsync(userSubscription, cancellationToken);
            await _unitOfWork.Commit();
            return Result<Guid>.Success(userSubscription.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}
