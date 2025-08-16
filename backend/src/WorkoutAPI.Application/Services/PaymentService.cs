using Mapster;
using Microsoft.Extensions.Logging;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Services;

public interface IPaymentService
{
    Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
    Task<PaymentResponse> UpdatePaymentAsync(Guid paymentId, UpdatePaymentRequest request);
    Task<PaymentResponse?> GetPaymentByIdAsync(Guid paymentId);
    Task<IEnumerable<PaymentResponse>> GetPaymentsByMemberIdAsync(Guid memberId);
    Task<IEnumerable<PaymentResponse>> GetPaymentsByStatusAsync(PaymentStatus status);
    Task<IEnumerable<PaymentResponse>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<bool> DeletePaymentAsync(Guid paymentId);
}

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IUnitOfWork unitOfWork, ILogger<PaymentService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
    {
        _logger.LogInformation("Creating new payment for member ID: {MemberId}", request.MemberId);

        // Check if member exists
        var member = await _unitOfWork.Members.GetByIdAsync(request.MemberId);
        if (member == null)
        {
            throw new ArgumentException($"Member with ID {request.MemberId} not found");
        }

        var payment = request.Adapt<Payment>();
        await _unitOfWork.Payments.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Payment created successfully with ID: {PaymentId}", payment.Id);

        var result = await GetPaymentByIdAsync(payment.Id);
        return result!;
    }

    public async Task<PaymentResponse> UpdatePaymentAsync(Guid paymentId, UpdatePaymentRequest request)
    {
        _logger.LogInformation("Updating payment with ID: {PaymentId}", paymentId);

        var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);
        if (payment == null)
        {
            throw new ArgumentException($"Payment with ID {paymentId} not found");
        }

        payment.Amount = request.Amount;
        payment.PaymentDate = request.PaymentDate;
        payment.Status = request.Status;
        payment.Description = request.Description;
        payment.SetUpdated();

        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Payment updated successfully with ID: {PaymentId}", paymentId);

        var result = await GetPaymentByIdAsync(paymentId);
        return result!;
    }

    public async Task<PaymentResponse?> GetPaymentByIdAsync(Guid paymentId)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);
        if (payment == null) return null;

        return new PaymentResponse(
            payment.Id,
            payment.MemberId,
            payment.Member.User.FullName,
            payment.Amount,
            payment.PaymentDate,
            payment.Status,
            payment.Description,
            payment.CreatedAt
        );
    }

    public async Task<IEnumerable<PaymentResponse>> GetPaymentsByMemberIdAsync(Guid memberId)
    {
        var payments = await _unitOfWork.Payments.GetPaymentsByMemberIdAsync(memberId);
        return payments.Select(p => new PaymentResponse(
            p.Id,
            p.MemberId,
            p.Member.User.FullName,
            p.Amount,
            p.PaymentDate,
            p.Status,
            p.Description,
            p.CreatedAt
        ));
    }

    public async Task<IEnumerable<PaymentResponse>> GetPaymentsByStatusAsync(PaymentStatus status)
    {
        var payments = await _unitOfWork.Payments.GetPaymentsByStatusAsync(status);
        return payments.Select(p => new PaymentResponse(
            p.Id,
            p.MemberId,
            p.Member.User.FullName,
            p.Amount,
            p.PaymentDate,
            p.Status,
            p.Description,
            p.CreatedAt
        ));
    }

    public async Task<IEnumerable<PaymentResponse>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var payments = await _unitOfWork.Payments.GetPaymentsByDateRangeAsync(startDate, endDate);
        return payments.Select(p => new PaymentResponse(
            p.Id,
            p.MemberId,
            p.Member.User.FullName,
            p.Amount,
            p.PaymentDate,
            p.Status,
            p.Description,
            p.CreatedAt
        ));
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _unitOfWork.Payments.GetTotalRevenueAsync();
    }

    public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.Payments.GetRevenueByDateRangeAsync(startDate, endDate);
    }

    public async Task<bool> DeletePaymentAsync(Guid paymentId)
    {
        _logger.LogInformation("Deleting payment with ID: {PaymentId}", paymentId);

        var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);
        if (payment == null)
        {
            return false;
        }

        _unitOfWork.Payments.Remove(payment);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Payment deleted successfully with ID: {PaymentId}", paymentId);

        return true;
    }
}