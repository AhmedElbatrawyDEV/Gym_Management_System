namespace WorkoutAPI.Application.DTOs;

public class SubscriptionPlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public List<string> Features { get; set; } = new();
    public bool IsActive { get; set; }
}
