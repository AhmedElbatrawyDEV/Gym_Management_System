using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.ProcessPayment;
public class ProcessPaymentCommand : BaseCommand
{
    public Guid PaymentId { get; init; }
    public string TransactionId { get; init; } = string.Empty;
}
