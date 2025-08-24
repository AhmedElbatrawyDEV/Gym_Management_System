using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Infrastructure.Interfaces;

namespace WorkoutAPI.Infrastructure.Repositories;

public class EfRepository<T> : IRepository<T> where T : BaseEntity {
    private readonly DbContext _db;
    private readonly DbSet<T> _set;

    public EfRepository(DbContext db) {
        _db = db;
        _set = _db.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
        => await _set.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    public async Task<List<T>> ListAsync()
        => await _set.Where(x => !x.IsDeleted).ToListAsync();

    public async Task<T> AddAsync(T entity, string? createdBy = null) {
        entity.SetCreated(createdBy);
        await _set.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity, string? updatedBy = null) {
        entity.SetUpdated(updatedBy);
        _set.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id, string? deletedBy = null) {
        var entity = await GetByIdAsync(id);
        if (entity is null) return;

        entity.SoftDelete(deletedBy);
        await UpdateAsync(entity, deletedBy);
    }

    public async Task HardDeleteAsync(Guid id) {
        var entity = await GetByIdAsync(id);
        if (entity is null) return;

        _set.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task RestoreAsync(Guid id, string? restoredBy = null) {
        var entity = await _set.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted);

        if (entity is null) return;

        entity.Restore();
        await UpdateAsync(entity, restoredBy);
    }
}