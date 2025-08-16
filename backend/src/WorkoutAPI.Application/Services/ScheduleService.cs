using Mapster;
using Microsoft.Extensions.Logging;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Services;

public interface IScheduleService
{
    Task<ScheduleResponse> CreateScheduleAsync(CreateScheduleRequest request);
    Task<ScheduleResponse> UpdateScheduleAsync(Guid scheduleId, UpdateScheduleRequest request);
    Task<ScheduleResponse?> GetScheduleByIdAsync(Guid scheduleId);
    Task<IEnumerable<ScheduleResponse>> GetSchedulesByTrainerIdAsync(Guid trainerId);
    Task<IEnumerable<ScheduleResponse>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<ScheduleResponse>> GetAvailableSchedulesAsync();
    Task<bool> DeleteScheduleAsync(Guid scheduleId);
    Task<bool> EnrollInScheduleAsync(Guid scheduleId);
    Task<bool> UnenrollFromScheduleAsync(Guid scheduleId);
}

public class ScheduleService : IScheduleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ScheduleService> _logger;

    public ScheduleService(IUnitOfWork unitOfWork, ILogger<ScheduleService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ScheduleResponse> CreateScheduleAsync(CreateScheduleRequest request)
    {
        _logger.LogInformation("Creating new schedule: {Title}", request.Title);

        // Validate trainer if provided
        if (request.TrainerId.HasValue)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(request.TrainerId.Value);
            if (trainer == null)
            {
                throw new ArgumentException($"Trainer with ID {request.TrainerId} not found");
            }
        }

        // Validate workout plan if provided
        if (request.WorkoutPlanId.HasValue)
        {
            var workoutPlan = await _unitOfWork.WorkoutPlans.GetByIdAsync(request.WorkoutPlanId.Value);
            if (workoutPlan == null)
            {
                throw new ArgumentException($"Workout plan with ID {request.WorkoutPlanId} not found");
            }
        }

        var schedule = request.Adapt<Schedule>();
        await _unitOfWork.Schedules.AddAsync(schedule);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Schedule created successfully with ID: {ScheduleId}", schedule.Id);

        var result = await GetScheduleByIdAsync(schedule.Id);
        return result!;
    }

    public async Task<ScheduleResponse> UpdateScheduleAsync(Guid scheduleId, UpdateScheduleRequest request)
    {
        _logger.LogInformation("Updating schedule with ID: {ScheduleId}", scheduleId);

        var schedule = await _unitOfWork.Schedules.GetByIdAsync(scheduleId);
        if (schedule == null)
        {
            throw new ArgumentException($"Schedule with ID {scheduleId} not found");
        }

        // Validate trainer if provided
        if (request.TrainerId.HasValue)
        {
            var trainer = await _unitOfWork.Trainers.GetByIdAsync(request.TrainerId.Value);
            if (trainer == null)
            {
                throw new ArgumentException($"Trainer with ID {request.TrainerId} not found");
            }
        }

        // Validate workout plan if provided
        if (request.WorkoutPlanId.HasValue)
        {
            var workoutPlan = await _unitOfWork.WorkoutPlans.GetByIdAsync(request.WorkoutPlanId.Value);
            if (workoutPlan == null)
            {
                throw new ArgumentException($"Workout plan with ID {request.WorkoutPlanId} not found");
            }
        }

        schedule.Title = request.Title;
        schedule.Description = request.Description;
        schedule.StartTime = request.StartTime;
        schedule.EndTime = request.EndTime;
        schedule.TrainerId = request.TrainerId;
        schedule.WorkoutPlanId = request.WorkoutPlanId;
        schedule.Capacity = request.Capacity;
        schedule.EnrolledCount = request.EnrolledCount;
        schedule.SetUpdated();

        _unitOfWork.Schedules.Update(schedule);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Schedule updated successfully with ID: {ScheduleId}", scheduleId);

        var result = await GetScheduleByIdAsync(scheduleId);
        return result!;
    }

    public async Task<ScheduleResponse?> GetScheduleByIdAsync(Guid scheduleId)
    {
        var schedule = await _unitOfWork.Schedules.GetScheduleWithDetailsAsync(scheduleId);
        if (schedule == null) return null;

        return new ScheduleResponse(
            schedule.Id,
            schedule.Title,
            schedule.Description,
            schedule.StartTime,
            schedule.EndTime,
            schedule.TrainerId,
            schedule.Trainer?.User.FullName,
            schedule.WorkoutPlanId,
            schedule.WorkoutPlan?.Translations.FirstOrDefault()?.Name ?? "Unknown Plan",
            schedule.Capacity,
            schedule.EnrolledCount,
            schedule.CreatedAt
        );
    }

    public async Task<IEnumerable<ScheduleResponse>> GetSchedulesByTrainerIdAsync(Guid trainerId)
    {
        var schedules = await _unitOfWork.Schedules.GetSchedulesByTrainerIdAsync(trainerId);
        return schedules.Select(s => new ScheduleResponse(
            s.Id,
            s.Title,
            s.Description,
            s.StartTime,
            s.EndTime,
            s.TrainerId,
            s.Trainer?.User.FullName,
            s.WorkoutPlanId,
            s.WorkoutPlan?.Translations.FirstOrDefault()?.Name ?? "Unknown Plan",
            s.Capacity,
            s.EnrolledCount,
            s.CreatedAt
        ));
    }

    public async Task<IEnumerable<ScheduleResponse>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var schedules = await _unitOfWork.Schedules.GetSchedulesByDateRangeAsync(startDate, endDate);
        return schedules.Select(s => new ScheduleResponse(
            s.Id,
            s.Title,
            s.Description,
            s.StartTime,
            s.EndTime,
            s.TrainerId,
            s.Trainer?.User.FullName,
            s.WorkoutPlanId,
            s.WorkoutPlan?.Translations.FirstOrDefault()?.Name ?? "Unknown Plan",
            s.Capacity,
            s.EnrolledCount,
            s.CreatedAt
        ));
    }

    public async Task<IEnumerable<ScheduleResponse>> GetAvailableSchedulesAsync()
    {
        var schedules = await _unitOfWork.Schedules.GetAvailableSchedulesAsync();
        return schedules.Select(s => new ScheduleResponse(
            s.Id,
            s.Title,
            s.Description,
            s.StartTime,
            s.EndTime,
            s.TrainerId,
            s.Trainer?.User.FullName,
            s.WorkoutPlanId,
            s.WorkoutPlan?.Translations.FirstOrDefault()?.Name ?? "Unknown Plan",
            s.Capacity,
            s.EnrolledCount,
            s.CreatedAt
        ));
    }

    public async Task<bool> DeleteScheduleAsync(Guid scheduleId)
    {
        _logger.LogInformation("Deleting schedule with ID: {ScheduleId}", scheduleId);

        var schedule = await _unitOfWork.Schedules.GetByIdAsync(scheduleId);
        if (schedule == null)
        {
            return false;
        }

        _unitOfWork.Schedules.Remove(schedule);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Schedule deleted successfully with ID: {ScheduleId}", scheduleId);

        return true;
    }

    public async Task<bool> EnrollInScheduleAsync(Guid scheduleId)
    {
        var schedule = await _unitOfWork.Schedules.GetByIdAsync(scheduleId);
        if (schedule == null || schedule.EnrolledCount >= schedule.Capacity)
        {
            return false;
        }

        schedule.EnrolledCount++;
        schedule.SetUpdated();

        _unitOfWork.Schedules.Update(schedule);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnenrollFromScheduleAsync(Guid scheduleId)
    {
        var schedule = await _unitOfWork.Schedules.GetByIdAsync(scheduleId);
        if (schedule == null || schedule.EnrolledCount <= 0)
        {
            return false;
        }

        schedule.EnrolledCount--;
        schedule.SetUpdated();

        _unitOfWork.Schedules.Update(schedule);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

