using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface IScheduleRepository : IRepository<Schedule> {
    Task<IEnumerable<Schedule>> GetSchedulesByTrainerIdAsync(Guid trainerId);
    Task<IEnumerable<Schedule>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Schedule>> GetAvailableSchedulesAsync();
    Task<Schedule?> GetScheduleWithDetailsAsync(Guid scheduleId);
}

