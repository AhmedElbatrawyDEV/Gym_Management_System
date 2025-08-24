
// Entities
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums.WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Events;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Domain.Aggregates;

// USER AGGREGATE ROOT
public class User : AggregateRoot<User> {
    private readonly List<UserSubscription> _subscriptions = new();
    private readonly List<WorkoutSession> _workoutSessions = new();

    public PersonalInfo PersonalInfo { get; private set; }
    public ContactInfo ContactInfo { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public UserStatus Status { get; private set; }
    public string MembershipNumber { get; private set; }
    public Language PreferredLanguage { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<UserSubscription> Subscriptions => _subscriptions.AsReadOnly();
    public IReadOnlyCollection<WorkoutSession> WorkoutSessions => _workoutSessions.AsReadOnly();
    public static User CreateNew(PersonalInfo personalInfo, ContactInfo contactInfo, Language preferredLanguage) {
        var user = BaseFactory.Create();
        user.PersonalInfo = personalInfo ?? throw new ArgumentNullException(nameof(personalInfo));
        user.ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        user.PreferredLanguage = preferredLanguage;
        user.Status = UserStatus.Active;
        user.MembershipNumber = user.GenerateMembershipNumber();
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        user.AddEvent(new UserRegisteredEvent(user.Guid, contactInfo.Email));
        return user;
    }

    public void UpdatePersonalInfo(PersonalInfo personalInfo) {
        PersonalInfo = personalInfo ?? throw new ArgumentNullException(nameof(personalInfo));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(ContactInfo contactInfo) {
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetProfileImage(string imageUrl) {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty", nameof(imageUrl));
        ProfileImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate() {
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend(string reason) {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Suspension reason is required", nameof(reason));
        Status = UserStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SubscribeTo(SubscriptionPlan plan, DateTime startDate) {
        if (plan == null) throw new ArgumentNullException(nameof(plan));

        var endDate = startDate.AddDays(plan.DurationDays);
        var subscription = UserSubscription.CreateNew(Guid, plan.Guid, new DateRange(startDate, endDate));
        _subscriptions.Add(subscription);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasActiveSubscription() {
        return _subscriptions.Any(s => s.IsActive);
    }

    private string GenerateMembershipNumber() {
        return $"MEM{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
    }
}
