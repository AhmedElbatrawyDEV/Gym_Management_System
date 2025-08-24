using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession> {
    public void Configure(EntityTypeBuilder<WorkoutSession> builder) {
        builder.ToTable("WorkoutSessions");

        builder.HasKey(ws => ws.Id);

        builder.Property(ws => ws.Notes)
            .HasMaxLength(1000);

        builder.Property(ws => ws.CreatedBy)
            .HasMaxLength(255);

        builder.Property(ws => ws.UpdatedBy)
            .HasMaxLength(255);

        // Configure backing fields for private setters
        builder.Property(ws => ws.UserId)
            .HasField("_userId");

        builder.Property(ws => ws.WorkoutPlanId)
            .HasField("_workoutPlanId");

        builder.Property(ws => ws.StartTime)
            .HasField("_startTime");

        builder.Property(ws => ws.EndTime)
            .HasField("_endTime");

        builder.Property(ws => ws.Status)
            .HasField("_status");

        builder.Property(ws => ws.CompletedExercises)
            .HasField("_completedExercises");

        builder.Property(ws => ws.TotalExercises)
            .HasField("_totalExercises");

        builder.Property(ws => ws.Notes)
            .HasField("_notes");

        // Relationships
        builder.HasOne(ws => ws.User)
            .WithMany(u => u.WorkoutSessions)
            .HasForeignKey(ws => ws.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ws => ws.WorkoutPlan)
            .WithMany(wp => wp.WorkoutSessions)
            .HasForeignKey(ws => ws.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<WorkoutSessionExercise>()
            .WithOne(wes => wes.WorkoutSession)
            .HasForeignKey(wes => wes.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore domain events
        builder.Ignore(ws => ws.DomainEvents);
    }
}

