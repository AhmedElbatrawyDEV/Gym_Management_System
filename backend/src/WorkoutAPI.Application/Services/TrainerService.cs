using Mapster;
using Microsoft.Extensions.Logging;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Services;

public interface ITrainerService
{
    Task<TrainerResponse> CreateTrainerAsync(CreateTrainerRequest request);
    Task<TrainerResponse> UpdateTrainerAsync(Guid trainerId, UpdateTrainerRequest request);
    Task<TrainerResponse?> GetTrainerByIdAsync(Guid trainerId);
    Task<TrainerResponse?> GetTrainerByUserIdAsync(Guid userId);
    Task<IEnumerable<TrainerResponse>> GetAvailableTrainersAsync();
    Task<IEnumerable<TrainerResponse>> GetTrainersBySpecializationAsync(string specialization);
    Task<bool> DeleteTrainerAsync(Guid trainerId);
}

public class TrainerService : ITrainerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TrainerService> _logger;

    public TrainerService(IUnitOfWork unitOfWork, ILogger<TrainerService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TrainerResponse> CreateTrainerAsync(CreateTrainerRequest request)
    {
        _logger.LogInformation("Creating new trainer for user ID: {UserId}", request.UserId);

        // Check if user exists
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {request.UserId} not found");
        }

        // Check if trainer already exists for this user
        var existingTrainer = await _unitOfWork.Trainers.GetByUserIdAsync(request.UserId);
        if (existingTrainer != null)
        {
            throw new InvalidOperationException($"Trainer already exists for user ID {request.UserId}");
        }

        var trainer = request.Adapt<Trainer>();
        await _unitOfWork.Trainers.AddAsync(trainer);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Trainer created successfully with ID: {TrainerId}", trainer.Id);

        var result = await GetTrainerByIdAsync(trainer.Id);
        return result!;
    }

    public async Task<TrainerResponse> UpdateTrainerAsync(Guid trainerId, UpdateTrainerRequest request)
    {
        _logger.LogInformation("Updating trainer with ID: {TrainerId}", trainerId);

        var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId);
        if (trainer == null)
        {
            throw new ArgumentException($"Trainer with ID {trainerId} not found");
        }

        trainer.Specialization = request.Specialization;
        trainer.Certification = request.Certification;
        trainer.HourlyRate = request.HourlyRate;
        trainer.IsAvailable = request.IsAvailable;
        trainer.SetUpdated();

        _unitOfWork.Trainers.Update(trainer);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Trainer updated successfully with ID: {TrainerId}", trainerId);

        var result = await GetTrainerByIdAsync(trainerId);
        return result!;
    }

    public async Task<TrainerResponse?> GetTrainerByIdAsync(Guid trainerId)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId);
        if (trainer == null) return null;

        return new TrainerResponse(
            trainer.Id,
            trainer.UserId,
            trainer.User.FirstName,
            trainer.User.LastName,
            trainer.User.Email,
            trainer.Specialization,
            trainer.Certification,
            trainer.HourlyRate,
            trainer.IsAvailable,
            trainer.CreatedAt
        );
    }

    public async Task<TrainerResponse?> GetTrainerByUserIdAsync(Guid userId)
    {
        var trainer = await _unitOfWork.Trainers.GetByUserIdAsync(userId);
        if (trainer == null) return null;

        return new TrainerResponse(
            trainer.Id,
            trainer.UserId,
            trainer.User.FirstName,
            trainer.User.LastName,
            trainer.User.Email,
            trainer.Specialization,
            trainer.Certification,
            trainer.HourlyRate,
            trainer.IsAvailable,
            trainer.CreatedAt
        );
    }

    public async Task<IEnumerable<TrainerResponse>> GetAvailableTrainersAsync()
    {
        var trainers = await _unitOfWork.Trainers.GetAvailableTrainersAsync();
        return trainers.Select(t => new TrainerResponse(
            t.Id,
            t.UserId,
            t.User.FirstName,
            t.User.LastName,
            t.User.Email,
            t.Specialization,
            t.Certification,
            t.HourlyRate,
            t.IsAvailable,
            t.CreatedAt
        ));
    }

    public async Task<IEnumerable<TrainerResponse>> GetTrainersBySpecializationAsync(string specialization)
    {
        var trainers = await _unitOfWork.Trainers.GetTrainersBySpecializationAsync(specialization);
        return trainers.Select(t => new TrainerResponse(
            t.Id,
            t.UserId,
            t.User.FirstName,
            t.User.LastName,
            t.User.Email,
            t.Specialization,
            t.Certification,
            t.HourlyRate,
            t.IsAvailable,
            t.CreatedAt
        ));
    }

    public async Task<bool> DeleteTrainerAsync(Guid trainerId)
    {
        _logger.LogInformation("Deleting trainer with ID: {TrainerId}", trainerId);

        var trainer = await _unitOfWork.Trainers.GetByIdAsync(trainerId);
        if (trainer == null)
        {
            return false;
        }

        // Set as unavailable instead of hard delete
        trainer.IsAvailable = false;
        trainer.SetUpdated();

        _unitOfWork.Trainers.Update(trainer);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Trainer soft deleted successfully with ID: {TrainerId}", trainerId);

        return true;
    }
}

