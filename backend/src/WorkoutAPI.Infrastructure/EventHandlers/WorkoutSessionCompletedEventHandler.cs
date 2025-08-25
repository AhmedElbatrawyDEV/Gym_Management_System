using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutAPI.Domain.Events;

namespace WorkoutAPI.Infrastructure.EventHandlers;

public class WorkoutSessionCompletedEventHandler : INotificationHandler<WorkoutSessionCompletedEvent>
{
    private readonly ILogger<WorkoutSessionCompletedEventHandler> _logger;

    public WorkoutSessionCompletedEventHandler(ILogger<WorkoutSessionCompletedEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(WorkoutSessionCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Workout session completed: {SessionId} for user {UserId}, duration: {Duration}",
            notification.SessionId, notification.UserId, notification.Duration);

        // Here you could add logic for:
        // - Updating user statistics
        // - Awarding points/achievements
        // - Sending completion notifications
        // - Analytics tracking

        await Task.CompletedTask;
    }
}
