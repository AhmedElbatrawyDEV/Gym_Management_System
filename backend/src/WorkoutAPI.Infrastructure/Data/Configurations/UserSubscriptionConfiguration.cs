using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        // Table configuration
        builder.ToTable("UserSubscriptions");

        // Primary key
        builder.HasKey(us => us.Id);
        builder.Property(us => us.Id).ValueGeneratedNever();

        // Properties
        builder.Property(us => us.UserId).IsRequired();
        builder.Property(us => us.SubscriptionPlanId).IsRequired();
        builder.Property(us => us.Status).IsRequired();
        builder.Property(us => us.CreatedAt).IsRequired();

        // Indexes
        builder.HasIndex(us => us.UserId);
        builder.HasIndex(us => us.SubscriptionPlanId);
        builder.HasIndex(us => us.Status);

        // Foreign key relationships
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<SubscriptionPlan>()
            .WithMany()
            .HasForeignKey(us => us.SubscriptionPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// =====