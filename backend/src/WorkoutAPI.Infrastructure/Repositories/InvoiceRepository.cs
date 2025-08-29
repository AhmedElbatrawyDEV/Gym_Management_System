using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
{
    private readonly WorkoutDbContext _context;

    public InvoiceRepository(WorkoutDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Invoices.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Invoices.ToListAsync(cancellationToken);
    }

    public async Task<Invoice> AddAsync(Invoice entity, CancellationToken cancellationToken = default)
    {
        await _context.Invoices.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<Invoice> UpdateAsync(Invoice entity, CancellationToken cancellationToken = default)
    {
        _context.Invoices.Update(entity);
        return await Task.FromResult(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.Invoices.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Invoices.AnyAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Invoices
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetByStatusAsync(Domain.Enums.InvoiceStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Invoices
            .Where(i => i.Status == status)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync(DateTime currentDate, CancellationToken cancellationToken = default)
    {
        // Assuming overdue invoices are those that are sent/viewed but not paid and created more than 30 days ago
        var overdueDate = currentDate.AddDays(-30);
        return await _context.Invoices
            .Where(i => (i.Status == Domain.Enums.InvoiceStatus.Sent || i.Status == Domain.Enums.InvoiceStatus.Viewed) &&
                        i.CreatedAt < overdueDate)
            .ToListAsync(cancellationToken);
    }
    public async Task<(IEnumerable<Invoice> invoices, int totalCount)> GetPaginatedAsync(
    int pageNumber,
    int pageSize,
    Guid? userId,
    InvoiceStatus? status,
    CancellationToken cancellationToken)
    {
        var query = _dbSet.AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(i => i.UserId == userId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var invoices = await query
            .OrderByDescending(i => i.CreatedAt) // Most recent first
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (invoices, totalCount);
    }

}
