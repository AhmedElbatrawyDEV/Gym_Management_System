using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Infrastructure.Data;
using WorkoutAPI.Infrastructure.Interfaces;

namespace WorkoutAPI.Infrastructure.Repositories;
public class ExerciseRepository : EfRepository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(AppDbContext db) : base(db) {}
}