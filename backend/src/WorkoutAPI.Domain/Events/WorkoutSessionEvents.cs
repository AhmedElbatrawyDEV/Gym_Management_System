namespace WorkoutAPI.Domain.Events;


public class UserRegisteredEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }

    public UserRegisteredEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}

public class WorkoutSessionCompletedEvent : DomainEvent
{

    public Guid SessionId { get; }
    public Guid UserId { get; }
    public TimeSpan Duration { get; }

    public WorkoutSessionCompletedEvent(Guid sessionId, Guid userId, TimeSpan duration)
    {
        SessionId = sessionId;
        UserId = userId;
        Duration = duration;
    }
}

public class PaymentProcessedEvent : DomainEvent
{
    public Guid PaymentId { get; }
    public Guid UserId { get; }
    public decimal Amount { get; }

    public PaymentProcessedEvent(Guid paymentId, Guid userId, decimal amount)
    {
        PaymentId = paymentId;
        UserId = userId;
        Amount = amount;
    }
}

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

