using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.SubscribeToPlan
{

    public class SubscribeToPlanCommandHandler : IRequestHandler<SubscribeToPlanCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscribeToPlanCommandHandler(
            IUserRepository userRepository,
            ISubscriptionPlanRepository subscriptionPlanRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SubscribeToPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginAsync();

                var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken) ?? throw new NotFoundException("User not found", request.UserId);

                var plan = await _subscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanId, cancellationToken) ?? throw new NotFoundException("Subscription plan not found", request.SubscriptionPlanId);

                user.SubscribeTo(plan, request.StartDate);

                await _userRepository.UpdateAsync(user, cancellationToken);
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
}
