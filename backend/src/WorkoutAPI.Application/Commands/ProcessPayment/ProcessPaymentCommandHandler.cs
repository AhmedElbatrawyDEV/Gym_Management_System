using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Interfaces;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.ProcessPayment;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public ProcessPaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IUserRepository userRepository)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken) ?? throw new NotFoundException("Payment", request.PaymentId);

        payment.ProcessPayment(request.TransactionId);

        await _unitOfWork.BeginAsync();
        try
        {
            await _paymentRepository.UpdateAsync(payment, cancellationToken);

            // Send payment confirmation email
            var user = await _userRepository.GetByIdAsync(payment.UserId, cancellationToken);
            if (user != null)
            {
                await _emailService.SendPaymentConfirmationAsync(
                    user.ContactInfo.Email,
                    user.PersonalInfo.FirstName,
                    payment.Amount.Amount,
                    request.TransactionId);
            }

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
