
// Entities
using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Entities;

public class Translation : Entity<Translation, Guid> {
    public string EntityType { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public string Culture { get; private set; } = "en";
    public string Field { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;

    private Translation() { } // EF Core

    public static Translation CreateNew(string entityType, Guid entityId, string culture, string field, string value) {
        return new Translation {
            Id = Guid.NewGuid(),
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType)),
            EntityId = entityId,
            Culture = culture ?? throw new ArgumentNullException(nameof(culture)),
            Field = field ?? throw new ArgumentNullException(nameof(field)),
            Value = value ?? throw new ArgumentNullException(nameof(value))
        };
    }

    public void UpdateValue(string newValue) {
        Value = newValue ?? throw new ArgumentNullException(nameof(newValue));
    }
}