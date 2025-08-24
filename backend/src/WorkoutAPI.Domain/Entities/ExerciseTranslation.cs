using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class ExerciseTranslation : BaseEntity {
    public Guid ExerciseId { get; set; }
    public Language Language { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Instructions { get; set; }

    // Navigation Properties
    public virtual Exercise Exercise { get; set; } = null!;
}

