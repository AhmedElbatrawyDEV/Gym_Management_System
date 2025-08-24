using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Events;
public interface INotification {
}
public interface IMessage : INotification {
    Guid Id => Guid.NewGuid();

    DateTimeOffset TimeStamp => DateTimeOffset.UtcNow;
}
public interface IEvent : IMessage {
}

public interface IDomainEvent : IEvent {
}

public abstract class DomainEvent : IDomainEvent {
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid EventId { get; } = Guid.NewGuid();
}

public abstract record AggregateDomainEvent<T> : IDomainEvent where T : IAggregateRoot {
    public T Aggregate { get; }

    protected AggregateDomainEvent(T aggregate) {
        Aggregate = aggregate;
    }
}

public record AggregateCreatedEvent<T> : AggregateDomainEvent<T> where T : IAggregateRoot {
    public AggregateCreatedEvent(T aggregate)
        : base(aggregate) {
    }
}