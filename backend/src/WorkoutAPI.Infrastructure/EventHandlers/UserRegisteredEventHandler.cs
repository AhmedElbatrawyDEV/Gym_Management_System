using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutAPI.Domain.Events;

namespace WorkoutAPI.Infrastructure.EventHandlers;

public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredEventHandler> _logger;

    public UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User registered: {UserId} with email {Email}",
            notification.UserId, notification.Email);

        // Here you could add logic for:
        // - Sending welcome email
        // - Creating default preferences
        // - Logging to analytics
        // - Triggering other business processes

        await Task.CompletedTask;
    }
}
