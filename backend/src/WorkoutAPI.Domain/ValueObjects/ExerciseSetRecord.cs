using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.ValueObjects;

public class ExerciseSetRecord : BaseEntity
{
    public Guid WorkoutExerciseSessionId { get; set; }
    public int SetNumber { get; set; }
    public int ActualReps { get; set; }
    public decimal ActualWeight { get; set; }
    public TimeSpan ActualRestTime { get; set; }
    public DateTime CompletedAt { get; set; }
    public string? Notes { get; set; }
    
    // Navigation Properties
    public virtual WorkoutExerciseSession WorkoutExerciseSession { get; set; } = null!;
    
    // Constructors
    private ExerciseSetRecord() { } // For EF Core
    
    public ExerciseSetRecord(int setNumber, int reps, decimal weight, TimeSpan restTime, DateTime completedAt)
    {
        SetNumber = setNumber;
        ActualReps = reps;
        ActualWeight = weight;
        ActualRestTime = restTime;
        CompletedAt = completedAt;
    }
}

