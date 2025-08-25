using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository {
    public PaymentRepository(WorkoutDbContext context) : base(context) { }

    public async Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payment?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default) {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.TransactionId == transactionId, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default) {
        return await _dbSet
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default) {
        var query = _dbSet.Where(p => p.Status == PaymentStatus.Completed);

        if (startDate.HasValue)
            query = query.Where(p => p.PaymentDate >= startDate);

        if (endDate.HasValue)
            query = query.Where(p => p.PaymentDate <= endDate);

        return await query.SumAsync(p => p.Amount.Amount, cancellationToken);
    }
}
