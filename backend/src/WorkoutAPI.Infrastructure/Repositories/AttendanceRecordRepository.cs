using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class AttendanceRecordRepository : IAttendanceRecordRepository
{
    private readonly WorkoutDbContext _context;

    public AttendanceRecordRepository(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<AttendanceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AttendanceRecords.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<AttendanceRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AttendanceRecords.ToListAsync(cancellationToken);
    }

    public async Task<AttendanceRecord> AddAsync(AttendanceRecord entity, CancellationToken cancellationToken = default)
    {
        await _context.AttendanceRecords.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<AttendanceRecord> UpdateAsync(AttendanceRecord entity, CancellationToken cancellationToken = default)
    {
        _context.AttendanceRecords.Update(entity);
        return await Task.FromResult(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.AttendanceRecords.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AttendanceRecords.AnyAsync(ar => ar.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<AttendanceRecord>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AttendanceRecords
            .Where(ar => ar.UserId == userId)
            .OrderByDescending(ar => ar.CheckInTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AttendanceRecord>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.AttendanceRecords
            .Where(ar => ar.UserId == userId && ar.CheckInTime >= startDate && ar.CheckInTime <= endDate)
            .OrderByDescending(ar => ar.CheckInTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<AttendanceRecord?> GetCurrentCheckInAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AttendanceRecords
            .FirstOrDefaultAsync(ar => ar.UserId == userId && !ar.CheckOutTime.HasValue, cancellationToken);
    }

    public async Task<(IEnumerable<AttendanceRecord> Records, int TotalCount)> GetPaginatedAsync(
    int pageNumber,
    int pageSize,
    Guid userId,
    DateTime? fromDate = null,
    DateTime? toDate = null,
    CancellationToken cancellationToken = default)
    {
        var query = _context.AttendanceRecords
            .Where(ar => ar.UserId == userId);

        if (fromDate.HasValue && toDate.HasValue)
        {
            query = query.Where(ar => ar.CheckInTime >= fromDate.Value && ar.CheckInTime <= toDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var records = await query
            .OrderByDescending(ar => ar.CheckInTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (records, totalCount);
    }


}
