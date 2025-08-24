using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Application.Services;

namespace WorkoutAPI.Api.Controllers;

// PaymentController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase {
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService) {
        _paymentService = paymentService;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment(ProcessPaymentRequest request) {
        try
        {
            var result = await _paymentService.ProcessPaymentAsync(request);
            return Ok(result);
        } catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayment(Guid id) {
        try
        {
            var payment = await _paymentService.GetPaymentAsync(id);
            return Ok(payment);
        } catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserPayments(Guid userId) {
        var payments = await _paymentService.GetUserPaymentsAsync(userId);
        return Ok(payments);
    }

    [HttpGet("{paymentId}/invoice")]
    public async Task<IActionResult> GetPaymentInvoice(Guid paymentId) {
        try
        {
            var invoice = await _paymentService.GetInvoiceAsync(paymentId);
            return Ok(invoice);
        } catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("user/{userId}/invoices")]
    public async Task<IActionResult> GetUserInvoices(Guid userId) {
        var invoices = await _paymentService.GetUserInvoicesAsync(userId);
        return Ok(invoices);
    }
}
