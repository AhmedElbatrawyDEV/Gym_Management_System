using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutPlanTranslation : BaseEntity {
    public Guid WorkoutPlanId { get; set; }
    public Language Language { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Goals { get; set; }

    // Navigation Properties
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;
}

