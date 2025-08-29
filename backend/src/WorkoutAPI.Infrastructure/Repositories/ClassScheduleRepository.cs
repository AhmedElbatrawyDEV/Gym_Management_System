using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;
public class ClassScheduleRepository : BaseRepository<ClassSchedule>, IClassScheduleRepository
{
    private readonly WorkoutDbContext _context;

    public ClassScheduleRepository(WorkoutDbContext context):base(context)
    {
        _context = context;
    }

    public async Task<ClassSchedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ClassSchedules
            .Include(cs => cs.GymClass)
            .Include(cs => cs.Bookings)
            .FirstOrDefaultAsync(cs => cs.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ClassSchedule>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ClassSchedules
            .Include(cs => cs.GymClass)
            .ToListAsync(cancellationToken);
    }

    public async Task<ClassSchedule> AddAsync(ClassSchedule entity, CancellationToken cancellationToken = default)
    {
        await _context.ClassSchedules.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<ClassSchedule> UpdateAsync(ClassSchedule entity, CancellationToken cancellationToken = default)
    {
        _context.ClassSchedules.Update(entity);
        return await Task.FromResult(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.ClassSchedules.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ClassSchedules.AnyAsync(cs => cs.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ClassSchedule>> GetByGymClassIdAsync(Guid gymClassId, CancellationToken cancellationToken = default)
    {
        return await _context.ClassSchedules
            .Include(cs => cs.GymClass)
            .Where(cs => cs.GymClassId == gymClassId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClassSchedule>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.ClassSchedules
            .Include(cs => cs.GymClass)
            .Where(cs => cs.StartTime >= startDate && cs.EndTime <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClassSchedule>> GetUpcomingSchedulesAsync(int daysAhead = 7, CancellationToken cancellationToken = default)
    {
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(daysAhead);

        return await _context.ClassSchedules
            .Include(cs => cs.GymClass)
            .Where(cs => cs.StartTime >= startDate && cs.StartTime <= endDate)
            .OrderBy(cs => cs.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClassSchedule>> GetAvailableSchedulesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ClassSchedules
            .Include(cs => cs.GymClass)
            .Where(cs => cs.Status == Domain.Enums.ClassStatus.Scheduled && cs.HasAvailableSpots)
            .OrderBy(cs => cs.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClassSchedule>> GetByIdsAsync(IEnumerable<Guid> scheduleIds, CancellationToken cancellationToken = default)
    {
        if (scheduleIds == null || !scheduleIds.Any())
            return Enumerable.Empty<ClassSchedule>();

        return await _dbSet
            .Where(cs => scheduleIds.Contains(cs.Id))
            .ToListAsync(cancellationToken);
    }

}
