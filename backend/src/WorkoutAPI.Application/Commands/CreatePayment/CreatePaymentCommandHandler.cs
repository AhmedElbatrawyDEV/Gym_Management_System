using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Application.Commands.CreatePayment;

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
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var userExists = await _userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
            throw new NotFoundException("User", request.UserId);

        var amount = new Money(request.Amount, request.Currency);
        var payment = Payment.CreateNew(request.UserId, amount, request.PaymentMethod,
            request.Description, request.UserSubscriptionId);

        await _unitOfWork.BeginAsync();
        try
        {
            await _paymentRepository.AddAsync(payment, cancellationToken);
            await _unitOfWork.Commit();
            return Result<Guid>.Success(payment.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}
