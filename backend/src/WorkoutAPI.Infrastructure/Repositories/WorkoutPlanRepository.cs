using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class WorkoutPlanRepository : IWorkoutPlanRepository
{
    private readonly WorkoutDbContext _context;

    public WorkoutPlanRepository(WorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .FirstOrDefaultAsync(wp => wp.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<WorkoutPlan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkoutPlan> AddAsync(WorkoutPlan entity, CancellationToken cancellationToken = default)
    {
        await _context.WorkoutPlans.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<WorkoutPlan> UpdateAsync(WorkoutPlan entity, CancellationToken cancellationToken = default)
    {
        _context.WorkoutPlans.Update(entity);
        return await Task.FromResult(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.WorkoutPlans.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans.AnyAsync(wp => wp.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<WorkoutPlan>> GetByTypeAsync(Domain.Enums.WorkoutPlanType type, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .Where(wp => wp.Type == type && wp.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkoutPlan>> GetByDifficultyAsync(Domain.Enums.DifficultyLevel difficulty, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .Where(wp => wp.DifficultyLevel == difficulty && wp.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkoutPlan>> GetActivePlansAsync(CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .Where(wp => wp.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkoutPlan>> GetByCreatorAsync(Guid creatorId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .Where(wp => wp.CreatedBy == creatorId)
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<WorkoutPlan>> GetFilteredAsync(
    Guid? createdBy,
    WorkoutPlanType? type,
    DifficultyLevel? difficulty,
    bool? activeOnly,
    CancellationToken cancellationToken = default)
    {
        var query = _context.WorkoutPlans
            .Include(wp => wp.Exercises)
            .AsQueryable();

        if (createdBy.HasValue)
        {
            query = query.Where(wp => wp.CreatedBy == createdBy.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(wp => wp.Type == type.Value);
        }

        if (difficulty.HasValue)
        {
            query = query.Where(wp => wp.DifficultyLevel == difficulty.Value);
        }

        if (activeOnly.HasValue && activeOnly.Value)
        {
            query = query.Where(wp => wp.IsActive);
        }

        return await query.ToListAsync(cancellationToken);
    }

}
