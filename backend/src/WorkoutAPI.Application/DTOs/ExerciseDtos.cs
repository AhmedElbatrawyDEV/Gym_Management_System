using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public record ExerciseResponse(
    Guid Id,
    string Code,
    ExerciseType Type,
    MuscleGroup PrimaryMuscleGroup,
    MuscleGroup? SecondaryMuscleGroup,
    DifficultyLevel Difficulty,
    string? IconName,
    bool IsActive,
    string Name,
    string? Description,
    string? Instructions
);

public record ExerciseTranslationResponse(
    Language Language,
    string Name,
    string? Description,
    string? Instructions
);

public record CreateExerciseRequest(
    string Code,
    ExerciseType Type,
    MuscleGroup PrimaryMuscleGroup,
    MuscleGroup? SecondaryMuscleGroup,
    DifficultyLevel Difficulty,
    string? IconName,
    List<CreateExerciseTranslationRequest> Translations
);

public record CreateExerciseTranslationRequest(
    Language Language,
    string Name,
    string? Description,
    string? Instructions
);

public record WorkoutExerciseResponse(
    Guid Id,
    string Code,
    ExerciseType Type,
    MuscleGroup PrimaryMuscleGroup,
    MuscleGroup? SecondaryMuscleGroup,
    DifficultyLevel Difficulty,
    string? IconName,
    string Name,
    string? Description,
    string? Instructions,
    int Order,
    int DefaultSets,
    int DefaultReps,
    TimeSpan DefaultRestTime,
    string? Notes
);

