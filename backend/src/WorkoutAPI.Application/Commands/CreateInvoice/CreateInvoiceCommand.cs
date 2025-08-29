using FluentValidation;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.CreateInvoice;
public class CreateInvoiceCommand : BaseCommand<Guid>
{
    public Guid UserId { get; init; }
    public Guid PaymentId { get; init; }
    public decimal Amount { get; init; }
    public decimal TaxAmount { get; init; }
}


public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("Payment ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Tax amount cannot be negative");
    }
}

