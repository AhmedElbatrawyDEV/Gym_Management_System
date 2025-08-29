using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CreateInvoice;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<Guid>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IUserRepository userRepository,
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _userRepository = userRepository;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var userExists = await _userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
            throw new NotFoundException("User", request.UserId);

        // Validate payment exists
        var paymentExists = await _paymentRepository.ExistsAsync(request.PaymentId, cancellationToken);
        if (!paymentExists)
            throw new NotFoundException("Payment", request.PaymentId);

        var invoice = Invoice.CreateNew(request.UserId, request.PaymentId, request.Amount, request.TaxAmount);

        await _unitOfWork.BeginAsync();
        try
        {
            await _invoiceRepository.AddAsync(invoice, cancellationToken);
            await _unitOfWork.Commit();
            return Result<Guid>.Success(invoice.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}

