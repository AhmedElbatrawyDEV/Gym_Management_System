using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.StartWorkoutSession;
public class StartWorkoutSessionCommand : BaseCommand
{
    public Guid Id { get; init; }
}
