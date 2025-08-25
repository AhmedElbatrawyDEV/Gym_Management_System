using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutAPI.Domain.Events;

namespace WorkoutAPI.Infrastructure.EventHandlers;

public class PaymentProcessedEventHandler : INotificationHandler<PaymentProcessedEvent>
{
    private readonly ILogger<PaymentProcessedEventHandler> _logger;

    public PaymentProcessedEventHandler(ILogger<PaymentProcessedEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(PaymentProcessedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Payment processed: {PaymentId} for user {UserId}, amount: {Amount}",
            notification.PaymentId, notification.UserId, notification.Amount);

        // Here you could add logic for:
        // - Sending payment confirmation email
        // - Updating subscription status
        // - Generating invoice
        // - Financial reporting

        await Task.CompletedTask;
    }
}
