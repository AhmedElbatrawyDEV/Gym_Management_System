using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;
public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<IEnumerable<Payment>> GetUserPaymentsAsync(Guid userId);
  
}
public class PaymentService : IPaymentService
{
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IRepository<User> _userRepository;

    public PaymentService(
        IPaymentProcessor paymentProcessor,
        IRepository<Payment> paymentRepository,
        IRepository<User> userRepository)
    {
        _paymentProcessor = paymentProcessor;
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return new PaymentResult(false, "User not found");

        var paymentResult = await _paymentProcessor.ProcessAsync(
            request.CardNumber, request.Expiry, request.CVV, request.Amount);

        if (!paymentResult.Success)
            return paymentResult;

        var payment = new Payment
        {
            UserId = request.UserId,
            Amount = request.Amount,
            PaymentDate = DateTime.UtcNow,
            TransactionId = paymentResult.TransactionId,
            Status = PaymentStatus.Completed
        };

        await _paymentRepository.AddAsync(payment);
        return new PaymentResult(true, "Payment processed successfully");
    }

    public async Task<IEnumerable<Payment>> GetUserPaymentsAsync(Guid userId)
    {
        return await _paymentRepository.GetWhereAsync(p => p.UserId == userId);
    }
}