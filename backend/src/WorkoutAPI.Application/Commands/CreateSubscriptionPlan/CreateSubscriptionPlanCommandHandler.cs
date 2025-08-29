using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Application.Commands.CreateSubscriptionPlan;

public class CreateSubscriptionPlanCommandHandler : IRequestHandler<CreateSubscriptionPlanCommand, Result<Guid>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriptionPlanCommandHandler(
        ISubscriptionPlanRepository subscriptionPlanRepository,
        IUnitOfWork unitOfWork)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        var price = new Money(request.Price, request.Currency);
        var subscriptionPlan = SubscriptionPlan.CreateNew(
            request.Name,
            request.Description,
            price,
            request.DurationDays,
            request.Features);

        await _unitOfWork.BeginAsync();
        try
        {
            await _subscriptionPlanRepository.AddAsync(subscriptionPlan, cancellationToken);
            await _unitOfWork.Commit();
            return Result<Guid>.Success(subscriptionPlan.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}