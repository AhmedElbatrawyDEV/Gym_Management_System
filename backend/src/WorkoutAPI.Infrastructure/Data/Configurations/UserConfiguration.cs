using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table configuration
        builder.ToTable("Users");

        // Primary key
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedNever();

        // Properties
        builder.Property(u => u.ProfileImageUrl).HasMaxLength(500);
        builder.Property(u => u.Status).IsRequired();
        builder.Property(u => u.MembershipNumber).HasMaxLength(50).IsRequired();
        builder.Property(u => u.PreferredLanguage).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired();

        // Indexes
        builder.HasIndex(u => u.MembershipNumber).IsUnique();

        // Value Objects are configured in DbContext

        // Navigation properties - Collections
        builder.HasMany<UserSubscription>()
            .WithOne()
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<WorkoutSession>()
            .WithOne()
            .HasForeignKey(ws => ws.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<Payment>()
            .WithOne()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ignore domain events (handled by base aggregate)
        builder.Ignore(u => u.UncommittedEvents);
        builder.Ignore(u => u.IsInitializing);
        builder.Ignore(u => u.IsNew);
    }
}
