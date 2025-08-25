using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        // Table configuration
        builder.ToTable("SubscriptionPlans");

        // Primary key
        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Id).ValueGeneratedNever();

        // Properties
        builder.Property(sp => sp.Name).HasMaxLength(100).IsRequired();
        builder.Property(sp => sp.Description).HasMaxLength(500).IsRequired();
        builder.Property(sp => sp.DurationDays).IsRequired();
        builder.Property(sp => sp.IsActive).IsRequired().HasDefaultValue(true);

        // Features as JSON array
        builder.Property(sp => sp.Features)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

        // Indexes
        builder.HasIndex(sp => sp.Name);
        builder.HasIndex(sp => sp.IsActive);
        builder.HasIndex(sp => sp.DurationDays);

        // Navigation properties
        builder.HasMany<UserSubscription>()
            .WithOne()
            .HasForeignKey(us => us.SubscriptionPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// =====