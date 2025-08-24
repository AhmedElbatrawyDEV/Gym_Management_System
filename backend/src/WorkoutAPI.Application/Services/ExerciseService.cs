using WorkoutAPI.Application.Abstractions;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Application.Services;
public class ExerciseService : IExerciseService {
    private readonly AppDbContext _db;
    public ExerciseService(AppDbContext db) { _db = db; }

    public async Task<ExerciseResponse> CreateAsync(CreateExerciseRequest request) {
        var e = new Exercise { Name = request.Name, Description = request.Description, TargetMuscle = request.TargetMuscle, Equipment = request.Equipment };
        _db.Exercises.Add(e);
        await _db.SaveChangesAsync();
        return Map(e);
    }

    public async Task DeleteAsync(Guid id) {
        var e = await _db.Exercises.FindAsync(id);
        if (e is null) return;
        _db.Exercises.Remove(e);
        await _db.SaveChangesAsync();
    }

    public async Task<ExerciseResponse?> GetAsync(Guid id)
        => await _db.Exercises.Where(x => x.Id == id).Select(e => Map(e)).FirstOrDefaultAsync();

    public async Task<List<ExerciseResponse>> ListAsync()
        => await _db.Exercises.Select(e => Map(e)).ToListAsync();

    public async Task<ExerciseResponse> UpdateAsync(Guid id, CreateExerciseRequest request) {
        var e = await _db.Exercises.FirstAsync(x => x.Id == id);
        e.Name = request.Name; e.Description = request.Description; e.TargetMuscle = request.TargetMuscle; e.Equipment = request.Equipment; e.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Map(e);
    }

    private static ExerciseResponse Map(Exercise e) => new ExerciseResponse(e.Id, e.Name, e.Description, e.TargetMuscle, e.Equipment);
}