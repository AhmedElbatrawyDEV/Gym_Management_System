
namespace WorkoutAPI.Application.DTOs {
    public record CreateSubscriptionPlanRequest(
        string Name,
        string Description,
        decimal Price,
        int DurationDays,
        List<string> Features
    );
    public record UpdateSubscriptionPlanRequest(
      string Name,
      string Description,
      decimal Price,
      int DurationDays,
      List<string> Features,
      bool IsActive
  );

    public record SubscriptionPlanResponse(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int DurationDays,
        List<string> Features,
        bool IsActive,
        DateTime CreatedAt
    );

    public record AssignSubscriptionRequest(
        Guid SubscriptionPlanId,
        DateTime? StartDate = null,
        bool AutoRenew = false
    );

    public record UserSubscriptionResponse(
        Guid Id,
        Guid UserId,
        SubscriptionPlanResponse Plan,
        DateTime StartDate,
        DateTime EndDate,
        SubscriptionStatus Status,
        bool AutoRenew,
        DateTime CreatedAt
    );

    public record ExtendSubscriptionRequest(
        int ExtensionDays
    );
}
