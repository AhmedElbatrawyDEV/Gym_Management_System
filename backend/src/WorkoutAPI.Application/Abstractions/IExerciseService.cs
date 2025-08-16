using WorkoutAPI.Application.DTOs;
namespace WorkoutAPI.Application.Abstractions;
public interface IExerciseService
{
    Task<List<ExerciseResponse>> ListAsync();
    Task<ExerciseResponse?> GetAsync(Guid id);
    Task<ExerciseResponse> CreateAsync(CreateExerciseRequest request);
    Task<ExerciseResponse> UpdateAsync(Guid id, CreateExerciseRequest request);
    Task DeleteAsync(Guid id);
}