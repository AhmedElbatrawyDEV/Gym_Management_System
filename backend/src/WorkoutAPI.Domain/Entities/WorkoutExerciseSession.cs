using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Domain.Entities;

public class WorkoutSessionExercise : BaseEntity
{
    public Guid WorkoutSessionId { get; set; }
    public Guid ExerciseId { get; set; }
    public int Order { get; set; }
    public bool IsCompleted { get; private set; } = false;
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public string? Notes { get; set; }
    
    private readonly List<ExerciseSetRecord> _sets = new();
    public virtual IReadOnlyCollection<ExerciseSetRecord> Sets => _sets.AsReadOnly();
    
    // Navigation Properties
    public virtual WorkoutSession WorkoutSession { get; set; } = null!;
    public virtual Exercise Exercise { get; set; } = null!;
    
    // Domain Methods
    public void StartExercise()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Exercise already completed");
            
        StartTime = DateTime.UtcNow;
    }
    
    public void CompleteSets(List<ExerciseSetRecord> sets)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Exercise already completed");
            
        _sets.Clear();
        _sets.AddRange(sets);
        
        IsCompleted = true;
        EndTime = DateTime.UtcNow;
    }
    
    public void AddSet(ExerciseSetRecord set)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot add sets to completed exercise");
            
        _sets.Add(set);
    }
}

