using System.Collections.ObjectModel;
using System.ComponentModel;
using WorkoutAPI.Domain.Events;

namespace WorkoutAPI.Domain.Common;
public interface IAggregateRoot : ISupportInitialize
{
    Guid Id { get; }

    IEnumerable<IDomainEvent> UncommittedEvents { get; }

    bool IsInitializing { get; }

    bool IsNew { get; }

    void AddEvent(IDomainEvent domainEvent, bool enforceWhileInitialization = false);

    void AddEvents(IEnumerable<IDomainEvent> domainEvents, bool enforceWhileInitialization = false);

    void ClearDomainEvents();

    bool IsEventAdded<TEvent>() where TEvent : IDomainEvent;

    bool IsEventAdded(IDomainEvent domainEvent);
}
public abstract class AggregateRoot<TAggregate> : IAggregateRoot, ISupportInitialize where TAggregate : AggregateRoot<TAggregate>, new()
{
    protected static class BaseFactory
    {
        public static TAggregate Create()
        {
            TAggregate val = new TAggregate();
            val.BeginInit();
            val.Id = Guid.NewGuid();
            val.AddEvent(new AggregateCreatedEvent<TAggregate>(val), enforceWhileInitialization: true);
            val.EndInit();
            return val;
        }
    }

    protected readonly ICollection<IDomainEvent> _UncommittedEvents;

    public Guid Id { get; protected set; }

    public IEnumerable<IDomainEvent> UncommittedEvents => new ReadOnlyCollection<IDomainEvent>(_UncommittedEvents.ToList());

    public bool IsInitializing { get; protected set; }

    public bool IsNew => IsEventAdded<AggregateCreatedEvent<TAggregate>>();

    protected AggregateRoot()
    {
        _UncommittedEvents = new Collection<IDomainEvent>();
    }

    public void AddEvent(IDomainEvent domainEvent, bool enforceWhileInitialization = false)
    {
        ArgumentNullException.ThrowIfNull(domainEvent, "domainEvent");
        if (!IsInitializing || enforceWhileInitialization)
        {
            _UncommittedEvents.Add(domainEvent);
        }
    }

    public void AddEvents(IEnumerable<IDomainEvent> domainEvents, bool enforceWhileInitialization = false)
    {
        ArgumentNullException.ThrowIfNull(domainEvents, "domainEvents");
        if (!(!IsInitializing || enforceWhileInitialization))
        {
            return;
        }

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            AddEvent(domainEvent);
        }
    }

    public void ClearDomainEvents()
    {
        _UncommittedEvents.Clear();
    }

    public bool IsEventAdded<TEvent>() where TEvent : IDomainEvent
    {
        if (_UncommittedEvents == null || _UncommittedEvents.Count == 0)
        {
            return false;
        }

        return _UncommittedEvents.OfType<TEvent>().Any();
    }

    public bool IsEventAdded(IDomainEvent domainEvent)
    {
        if (_UncommittedEvents == null || _UncommittedEvents.Count == 0)
        {
            return false;
        }

        return _UncommittedEvents.Any((IDomainEvent e) => object.Equals(e, domainEvent));
    }

    public virtual void BeginInit()
    {
        IsInitializing = true;
    }

    public virtual void EndInit()
    {
        IsInitializing = false;
    }
}

