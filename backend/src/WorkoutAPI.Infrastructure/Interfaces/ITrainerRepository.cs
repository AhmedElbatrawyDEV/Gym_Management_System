using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Domain.Interfaces;

public interface ITrainerRepository : IRepository<Trainer> {
    Task<Trainer?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Trainer>> GetAvailableTrainersAsync();
    Task<Trainer?> GetTrainerWithSchedulesAsync(Guid trainerId);
    Task<IEnumerable<Trainer>> GetTrainersBySpecializationAsync(string specialization);
}

