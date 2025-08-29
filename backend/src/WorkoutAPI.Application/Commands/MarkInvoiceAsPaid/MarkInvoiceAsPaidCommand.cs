using FluentValidation;
using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.MarkInvoiceAsPaid;


public class MarkInvoiceAsPaidCommand : BaseCommand
{
    public Guid InvoiceId { get; init; }
}
public class MarkInvoiceAsPaidCommandValidator : AbstractValidator<MarkInvoiceAsPaidCommand>
{
    public MarkInvoiceAsPaidCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty().WithMessage("Invoice ID is required");
    }
}