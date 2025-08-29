using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.CompleteWorkoutSession;
public class CompleteWorkoutSessionCommand : BaseCommand
{
    public Guid Id { get; init; }
    public string? Notes { get; init; }
}
