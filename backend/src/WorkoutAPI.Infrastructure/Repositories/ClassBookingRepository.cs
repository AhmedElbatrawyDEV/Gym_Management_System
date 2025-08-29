using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class ClassBookingRepository : BaseRepository<ClassBooking> , IClassBookingRepository
{
    private readonly WorkoutDbContext _context;

    public ClassBookingRepository(WorkoutDbContext context) :base(context)
    {
        _context = context;
    }

    public async Task<ClassBooking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ClassBookings
            .Include(cb => cb.ClassSchedule)
            .ThenInclude(cs => cs.GymClass)
            .FirstOrDefaultAsync(cb => cb.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ClassBooking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ClassBookings
            .Include(cb => cb.ClassSchedule)
            .ThenInclude(cs => cs.GymClass)
            .ToListAsync(cancellationToken);
    }

    public async Task<ClassBooking> AddAsync(ClassBooking entity, CancellationToken cancellationToken = default)
    {
        await _context.ClassBookings.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<ClassBooking> UpdateAsync(ClassBooking entity, CancellationToken cancellationToken = default)
    {
        _context.ClassBookings.Update(entity);
        return await Task.FromResult(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.ClassBookings.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ClassBookings.AnyAsync(cb => cb.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ClassBooking>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.ClassBookings
            .Include(cb => cb.ClassSchedule)
            .ThenInclude(cs => cs.GymClass)
            .Where(cb => cb.UserId == userId)
            .OrderByDescending(cb => cb.BookingDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClassBooking>> GetByClassScheduleIdAsync(Guid classScheduleId, CancellationToken cancellationToken = default)
    {
        return await _context.ClassBookings
            .Include(cb => cb.User)
            .Where(cb => cb.ClassScheduleId == classScheduleId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClassBooking>> GetByStatusAsync(Domain.Enums.BookingStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.ClassBookings
            .Include(cb => cb.ClassSchedule)
            .ThenInclude(cs => cs.GymClass)
            .Include(cb => cb.User)
            .Where(cb => cb.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClassBooking>> GetFilteredAsync(
      Guid? userId = null,
      Guid? classScheduleId = null,
      BookingStatus? status = null,
      DateTime? fromDate = null,
      DateTime? toDate = null,
      CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (userId.HasValue)
            query = query.Where(cb => cb.UserId == userId.Value);

        if (classScheduleId.HasValue)
            query = query.Where(cb => cb.ClassScheduleId == classScheduleId.Value);

        if (status.HasValue)
            query = query.Where(cb => cb.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(cb => cb.BookingDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(cb => cb.BookingDate <= toDate.Value);

        return await query
            .Include(cb => cb.User)
            .Include(cb => cb.ClassSchedule)
            .OrderByDescending(cb => cb.BookingDate)
            .ToListAsync(cancellationToken);
    }

}
