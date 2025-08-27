using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Interfaces;

public interface IClassBookingRepository : IRepository<ClassBooking>
{
    Task<IEnumerable<ClassBooking>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClassBooking>> GetByClassScheduleIdAsync(Guid classScheduleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClassBooking>> GetByStatusAsync(BookingStatus status, CancellationToken cancellationToken = default);
}
