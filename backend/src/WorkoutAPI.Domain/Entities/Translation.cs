using WorkoutAPI.Domain.Common;
namespace WorkoutAPI.Domain.Entities;
public class Translation : BaseEntity {
    public string EntityType { get; set; } = default!; // "Exercise" or "WorkoutPlan"
    public Guid EntityId { get; set; }
    public string Culture { get; set; } = "en";
    public string Field { get; set; } = default!; // "Name" or "Description"
    public string Value { get; set; } = default!;
}