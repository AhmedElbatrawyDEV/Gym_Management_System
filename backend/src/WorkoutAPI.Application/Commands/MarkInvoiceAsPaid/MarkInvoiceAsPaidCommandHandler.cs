using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.MarkInvoiceAsPaid;

public class MarkInvoiceAsPaidCommandHandler : IRequestHandler<MarkInvoiceAsPaidCommand, Result>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkInvoiceAsPaidCommandHandler(
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(MarkInvoiceAsPaidCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken) ?? throw new NotFoundException("Invoice", request.InvoiceId);

        invoice.MarkAsPaid();

        await _unitOfWork.BeginAsync();
        try
        {
            await _invoiceRepository.UpdateAsync(invoice, cancellationToken);
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
