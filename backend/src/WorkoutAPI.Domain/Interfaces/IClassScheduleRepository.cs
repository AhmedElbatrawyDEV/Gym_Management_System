using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface IClassScheduleRepository : IRepository<ClassSchedule>
{
    Task<IEnumerable<ClassSchedule>> GetByGymClassIdAsync(Guid gymClassId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClassSchedule>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClassSchedule>> GetUpcomingSchedulesAsync(int daysAhead = 7, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClassSchedule>> GetAvailableSchedulesAsync(CancellationToken cancellationToken = default);
}
