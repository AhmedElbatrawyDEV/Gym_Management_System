
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums.WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class Exercise : Entity<Exercise, Guid> {
    private readonly List<ExerciseTranslation> _translations = new();

    public string Code { get; private set; } = string.Empty;
    public ExerciseType Type { get; private set; }
    public MuscleGroup PrimaryMuscleGroup { get; private set; }
    public MuscleGroup? SecondaryMuscleGroup { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public string? IconName { get; private set; }
    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<ExerciseTranslation> Translations => _translations.AsReadOnly();

    private Exercise() { } // EF Core

    public static Exercise CreateNew(string code, ExerciseType type, MuscleGroup primaryMuscleGroup,
                                   DifficultyLevel difficulty, MuscleGroup? secondaryMuscleGroup = null,
                                   string? iconName = null) {
        return new Exercise {
            Id = Guid.NewGuid(),
            Code = code ?? throw new ArgumentNullException(nameof(code)),
            Type = type,
            PrimaryMuscleGroup = primaryMuscleGroup,
            SecondaryMuscleGroup = secondaryMuscleGroup,
            Difficulty = difficulty,
            IconName = iconName
        };
    }

    public void AddTranslation(Language language, string name, string? description = null, string? instructions = null) {
        if (_translations.Any(t => t.Language == language))
            throw new InvalidOperationException($"Translation for {language} already exists");

        var translation = ExerciseTranslation.CreateNew(Id, language, name, description, instructions);
        _translations.Add(translation);
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
