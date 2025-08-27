using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // Table configuration
        builder.ToTable("Payments");

        // Primary key
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        // Properties
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.UserSubscriptionId);
        builder.Property(p => p.PaymentMethod).IsRequired();
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.PaymentDate);
        builder.Property(p => p.TransactionId).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);

        // JSON column for metadata
        builder.Property(p => p.Metadata)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null));

        // Indexes
        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.TransactionId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.PaymentDate);

        // Foreign key relationships
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<UserSubscription>()
            .WithMany()
            .HasForeignKey(p => p.UserSubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Ignore domain events
        builder.Ignore(p => p.UncommittedEvents);
        builder.Ignore(p => p.IsInitializing);
        builder.Ignore(p => p.IsNew);
    }
}
