using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity {
    protected readonly WorkoutDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(WorkoutDbContext context) {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id) {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate) {
        return await _dbSet.SingleOrDefaultAsync(predicate);
    }

    public virtual async Task AddAsync(T entity) {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities) {
        await _dbSet.AddRangeAsync(entities);
    }

    public virtual void Update(T entity) {
        _dbSet.Update(entity);
    }

    public virtual void Remove(T entity) {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities) {
        _dbSet.RemoveRange(entities);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null) {
        if (predicate == null)
            return await _dbSet.CountAsync();

        return await _dbSet.CountAsync(predicate);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) {
        return await _dbSet.AnyAsync(predicate);
    }
}

