using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Interfaces;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetByStatusAsync(InvoiceStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync(DateTime currentDate, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Invoice> invoices, int totalCount)> GetPaginatedAsync(int pageNumber, int pageSize, Guid? userId, InvoiceStatus? status, CancellationToken cancellationToken);
}