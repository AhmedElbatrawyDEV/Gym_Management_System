using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
namespace WorkoutAPI.Application.Abstractions;
public interface IPaymentProcessor
{
    Task<PaymentResult> CaptureAsync(PaymentRequest request);
}