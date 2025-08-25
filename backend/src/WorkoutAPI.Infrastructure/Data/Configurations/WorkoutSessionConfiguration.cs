using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        // Table configuration
        builder.ToTable("WorkoutSessions");

        // Primary key
        builder.HasKey(ws => ws.Guid);
        builder.Property(ws => ws.Guid).ValueGeneratedNever();

        // Properties
        builder.Property(ws => ws.UserId).IsRequired();
        builder.Property(ws => ws.TrainerId);
        builder.Property(ws => ws.Title).HasMaxLength(200).IsRequired();
        builder.Property(ws => ws.StartTime).IsRequired();
        builder.Property(ws => ws.EndTime);
        builder.Property(ws => ws.Status).IsRequired();
        builder.Property(ws => ws.Notes).HasMaxLength(1000);

        // Indexes
        builder.HasIndex(ws => ws.UserId);
        builder.HasIndex(ws => ws.TrainerId);
        builder.HasIndex(ws => ws.StartTime);
        builder.HasIndex(ws => ws.Status);

        // Navigation properties
        builder.HasMany<WorkoutSessionExercise>()
            .WithOne()
            .HasForeignKey(wse => wse.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key relationships
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ws => ws.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Trainer>()
            .WithMany()
            .HasForeignKey(ws => ws.TrainerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Ignore domain events
        builder.Ignore(ws => ws.UncommittedEvents);
        builder.Ignore(ws => ws.IsInitializing);
        builder.Ignore(ws => ws.IsNew);
    }
}
