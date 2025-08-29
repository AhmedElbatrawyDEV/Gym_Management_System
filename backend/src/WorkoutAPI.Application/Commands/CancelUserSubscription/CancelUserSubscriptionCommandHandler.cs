using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CancelUserSubscription;

public class CancelUserSubscriptionCommandHandler : IRequestHandler<CancelUserSubscriptionCommand, Result>
{
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelUserSubscriptionCommandHandler(
        IUserSubscriptionRepository userSubscriptionRepository,
        IUnitOfWork unitOfWork)
    {
        _userSubscriptionRepository = userSubscriptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelUserSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var userSubscription = await _userSubscriptionRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("UserSubscription", request.Id);

        userSubscription.Cancel();

        await _unitOfWork.BeginAsync();
        try
        {
            await _userSubscriptionRepository.UpdateAsync(userSubscription, cancellationToken);
            await _unitOfWork.Commit();
            return Result.Success();
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}
