using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface ITrainerRepository : IRepository<Trainer> {
    Task<IEnumerable<Trainer>> GetAvailableTrainersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Trainer>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);
    Task<Trainer?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

