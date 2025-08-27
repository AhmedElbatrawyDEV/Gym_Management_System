namespace WorkoutAPI.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    Task SendWelcomeEmailAsync(string to, string firstName);
    Task SendWorkoutReminderAsync(string to, string firstName, DateTime sessionTime);
    Task SendPaymentConfirmationAsync(string to, string firstName, decimal amount, string transactionId);
}

