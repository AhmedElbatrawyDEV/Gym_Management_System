using WorkoutAPI.Domain.Common;

namespace WorkoutAPI.Domain.ValueObjects;

public class ContactInfo : ValueObject
{
    public string Email { get; private set; }
    public string? PhoneNumber { get; private set; }

    private ContactInfo() { } // EF Core

    public ContactInfo(string email, string? phoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        Email = email.ToLowerInvariant().Trim();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
        yield return PhoneNumber ?? string.Empty;
    }
}

