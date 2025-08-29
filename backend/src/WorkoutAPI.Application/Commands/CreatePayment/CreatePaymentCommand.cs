using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.Commands.CreatePayment;
public class CreatePaymentCommand : BaseCommand<Guid>
{
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "SAR";
    public PaymentMethod PaymentMethod { get; init; }
    public string? Description { get; init; }
    public Guid? UserSubscriptionId { get; init; }
}
