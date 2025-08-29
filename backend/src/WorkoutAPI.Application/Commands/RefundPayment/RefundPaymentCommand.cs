using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.RefundPayment;
public class RefundPaymentCommand : BaseCommand
{
    public Guid PaymentId { get; init; }
    public string Reason { get; init; } = string.Empty;
}
