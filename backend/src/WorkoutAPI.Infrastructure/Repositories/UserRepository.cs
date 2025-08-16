using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Infrastructure.Data;
using WorkoutAPI.Infrastructure.Interfaces;

namespace WorkoutAPI.Infrastructure.Repositories;
public class UserRepository : EfRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext db) : base(db) {}
}