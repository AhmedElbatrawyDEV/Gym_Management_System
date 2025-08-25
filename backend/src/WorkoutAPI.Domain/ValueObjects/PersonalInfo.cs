using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.ValueObjects;

// Value Objects
public class PersonalInfo : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }

    private PersonalInfo() { } // EF Core

    public PersonalInfo(string firstName, string lastName, DateTime dateOfBirth, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        if (dateOfBirth >= DateTime.Today)
            throw new ArgumentException("Date of birth must be in the past", nameof(dateOfBirth));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.UtcNow.Year - DateOfBirth.Year -
                     (DateTime.UtcNow.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName.ToLowerInvariant();
        yield return LastName.ToLowerInvariant();
        yield return DateOfBirth;
        yield return Gender;
    }
}

