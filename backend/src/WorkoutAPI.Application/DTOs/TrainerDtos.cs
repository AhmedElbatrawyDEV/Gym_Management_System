using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public record CreateTrainerRequest(
    Guid UserId,
    string Specialization,
    string Certification,
    decimal HourlyRate
);

public record UpdateTrainerRequest(
    string Specialization,
    string Certification,
    decimal HourlyRate,
    bool IsAvailable
);

public record TrainerResponse(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Specialization,
    string Certification,
    decimal HourlyRate,
    bool IsAvailable,
    DateTime CreatedAt
);


