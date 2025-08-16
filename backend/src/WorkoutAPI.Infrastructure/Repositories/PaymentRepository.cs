using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(WorkoutDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByMemberIdAsync(Guid memberId)
    {
        return await _dbSet
            .Include(p => p.Member)
                .ThenInclude(m => m.User)
            .Where(p => p.MemberId == memberId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
    {
        return await _dbSet
            .Include(p => p.Member)
                .ThenInclude(m => m.User)
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(p => p.Member)
                .ThenInclude(m => m.User)
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _dbSet
            .Where(p => p.Status == PaymentStatus.Completed)
            .SumAsync(p => p.Amount);
    }

    public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(p => p.Status == PaymentStatus.Completed && 
                       p.PaymentDate >= startDate && 
                       p.PaymentDate <= endDate)
            .SumAsync(p => p.Amount);
    }
}

