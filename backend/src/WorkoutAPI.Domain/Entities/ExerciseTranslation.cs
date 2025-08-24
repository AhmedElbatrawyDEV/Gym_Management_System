
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums.WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class ExerciseTranslation : Entity<ExerciseTranslation, Guid> {
    public Guid ExerciseId { get; private set; }
    public Language Language { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Instructions { get; private set; }

    private ExerciseTranslation() { } // EF Core

    public static ExerciseTranslation CreateNew(Guid exerciseId, Language language, string name,
                                              string? description = null, string? instructions = null) {
        return new ExerciseTranslation {
            Id = Guid.NewGuid(),
            ExerciseId = exerciseId,
            Language = language,
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            Description = description,
            Instructions = instructions
        };
    }
}
