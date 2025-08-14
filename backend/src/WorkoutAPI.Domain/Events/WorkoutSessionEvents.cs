namespace WorkoutAPI.Domain.Events;

public class WorkoutSessionStartedEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WorkoutSessionId { get; }
    public Guid WorkoutPlanId { get; }
    
    public WorkoutSessionStartedEvent(Guid userId, Guid workoutSessionId, Guid workoutPlanId)
    {
        UserId = userId;
        WorkoutSessionId = workoutSessionId;
        WorkoutPlanId = workoutPlanId;
    }
}

public class ExerciseCompletedEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WorkoutSessionId { get; }
    public Guid ExerciseId { get; }
    public int CompletedSets { get; }
    public decimal TotalWeight { get; }
    
    public ExerciseCompletedEvent(Guid userId, Guid workoutSessionId, Guid exerciseId, int completedSets, decimal totalWeight)
    {
        UserId = userId;
        WorkoutSessionId = workoutSessionId;
        ExerciseId = exerciseId;
        CompletedSets = completedSets;
        TotalWeight = totalWeight;
    }
}

public class WorkoutSessionCompletedEvent : DomainEvent
{
    public Guid UserId { get; }
    public Guid WorkoutSessionId { get; }
    public TimeSpan Duration { get; }
    public int CompletedExercises { get; }
    
    public WorkoutSessionCompletedEvent(Guid userId, Guid workoutSessionId, TimeSpan duration, int completedExercises)
    {
        UserId = userId;
        WorkoutSessionId = workoutSessionId;
        Duration = duration;
        CompletedExercises = completedExercises;
    }
}

