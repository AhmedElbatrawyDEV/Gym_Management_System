using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan> {
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder) {
        builder.ToTable("WorkoutPlans");

        builder.HasKey(wp => wp.Id);

        builder.Property(wp => wp.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(wp => wp.CreatedBy)
            .HasMaxLength(255);

        builder.Property(wp => wp.UpdatedBy)
            .HasMaxLength(255);

        // Relationships
        builder.HasMany(wp => wp.Translations)
            .WithOne(wpt => wpt.WorkoutPlan)
            .HasForeignKey(wpt => wpt.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(wp => wp.WorkoutPlanExercises)
            .WithOne(wpe => wpe.WorkoutPlan)
            .HasForeignKey(wpe => wpe.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(wp => wp.UserWorkoutPlans)
            .WithOne(uwp => uwp.WorkoutPlan)
            .HasForeignKey(uwp => uwp.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(wp => wp.WorkoutSessions)
            .WithOne(ws => ws.WorkoutPlan)
            .HasForeignKey(ws => ws.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

