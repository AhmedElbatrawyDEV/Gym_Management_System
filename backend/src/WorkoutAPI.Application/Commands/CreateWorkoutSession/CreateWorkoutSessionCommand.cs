using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.CreateWorkoutSession;

public class CreateWorkoutSessionCommand : BaseCommand<Guid>
{
    public Guid UserId { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
    public Guid? TrainerId { get; init; }
}
