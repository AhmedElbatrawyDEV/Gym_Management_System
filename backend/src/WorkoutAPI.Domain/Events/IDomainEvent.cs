using MediatR;
using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.Events;

public interface IMessage : INotification
{
    Guid Id => Guid.NewGuid();

    DateTimeOffset TimeStamp => DateTimeOffset.UtcNow;
}
public interface IEvent : IMessage, INotification
{
}

public interface IDomainEvent : IEvent, IMessage, INotification
{
}

public abstract class DomainEvent : IDomainEvent, IEvent, IMessage, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid EventId { get; } = Guid.NewGuid();
}

public abstract record AggregateDomainEvent<T> : IDomainEvent, IEvent, IMessage, INotification where T : IAggregateRoot
{
    public T Aggregate { get; }

    protected AggregateDomainEvent(T aggregate)
    {
        Aggregate = aggregate;
    }
}

public record AggregateCreatedEvent<T> : AggregateDomainEvent<T> where T : IAggregateRoot
{
    public AggregateCreatedEvent(T aggregate)
        : base(aggregate)
    {
    }
}