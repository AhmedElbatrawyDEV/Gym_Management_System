using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetInvoicesQuery;
public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, PaginatedResult<InvoiceDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUserRepository _userRepository;

    public GetInvoicesQueryHandler(
        IInvoiceRepository invoiceRepository,
        IUserRepository userRepository)
    {
        _invoiceRepository = invoiceRepository;
        _userRepository = userRepository;
    }

    public async Task<PaginatedResult<InvoiceDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var (invoices, totalCount) = await _invoiceRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.UserId,
            request.Status,
            cancellationToken);

        var userIds = invoices.Select(i => i.UserId).Distinct().ToList();
        var users = userIds.Any() ? await _userRepository.GetByIdsAsync(userIds, cancellationToken) : new List<User>();
        var userLookup = users.ToDictionary(u => u.Id, u => u.PersonalInfo.FullName);

        var invoiceDtos = invoices.Select(invoice => new InvoiceDto
        {
            Id = invoice.Id,
            UserId = invoice.UserId,
            UserName = userLookup.GetValueOrDefault(invoice.UserId, string.Empty),
            PaymentId = invoice.PaymentId,
            InvoiceNumber = invoice.InvoiceNumber,
            Amount = invoice.Amount,
            TaxAmount = invoice.TaxAmount,
            TotalAmount = invoice.TotalAmount,
            Status = invoice.Status,
            PaidAt = invoice.PaidAt,
            CreatedAt = invoice.CreatedAt
        }).ToList().AsReadOnly();

        return PaginatedResult<InvoiceDto>.Success(invoiceDtos, request.PageNumber, request.PageSize, totalCount);
    }
}
