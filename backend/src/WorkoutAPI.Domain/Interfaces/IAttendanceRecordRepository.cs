using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface IAttendanceRecordRepository : IRepository<AttendanceRecord>
{
    Task<IEnumerable<AttendanceRecord>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<AttendanceRecord?> GetCurrentCheckInAsync(Guid userId, CancellationToken cancellationToken = default);
}
