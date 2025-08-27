using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Features.Users.Commands.SubscribeToPlan
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

                var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
                if (user == null)
                    return Result.Failure("User not found");

                var plan = await _subscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanId, cancellationToken);
                if (plan == null)
                    return Result.Failure("Subscription plan not found");

                user.SubscribeTo(plan, request.StartDate);

                await _userRepository.UpdateAsync(user, cancellationToken);
                await _unitOfWork.Commit();

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                return Result.Failure($"Failed to subscribe to plan: {ex.Message}");
            }
        }
    }
}
