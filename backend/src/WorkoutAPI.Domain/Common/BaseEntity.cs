using System.ComponentModel.DataAnnotations;
using WorkoutAPI.Domain.Events;

namespace WorkoutAPI.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    [StringLength(255)]
    public string? CreatedBy { get; protected set; }

    [StringLength(255)]
    public string? UpdatedBy { get; protected set; }

    [StringLength(255)]
    public string? DeletedBy { get; protected set; }

    public bool IsDeleted { get; protected set; } = false;

    // Domain events
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void SetCreated(string? createdBy = null)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void SetUpdated(string? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void SoftDelete(string? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        SetUpdated(deletedBy);
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        SetUpdated();
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}