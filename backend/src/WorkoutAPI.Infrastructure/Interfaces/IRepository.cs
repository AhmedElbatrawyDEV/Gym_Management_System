using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Infrastructure.Interfaces;

public interface IRepository<T> where T : BaseEntity {
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> ListAsync();
    Task<T> AddAsync(T entity, string? createdBy = null);
    Task UpdateAsync(T entity, string? updatedBy = null);
    Task DeleteAsync(Guid id, string? deletedBy = null);
    Task HardDeleteAsync(Guid id);
    Task RestoreAsync(Guid id, string? restoredBy = null);
}