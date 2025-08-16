using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Interfaces;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IEnumerable<Payment>> GetPaymentsByMemberIdAsync(Guid memberId);
    Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
    Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
}

