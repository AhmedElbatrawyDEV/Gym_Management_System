using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class WorkoutSessionExerciseConfiguration : IEntityTypeConfiguration<WorkoutSessionExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutSessionExercise> builder)
    {
        // Table configuration
        builder.ToTable("WorkoutSessionExercises");

        // Primary key
        builder.HasKey(wse => wse.Id);
        builder.Property(wse => wse.Id).ValueGeneratedNever();

        // Properties
        builder.Property(wse => wse.WorkoutSessionId).IsRequired();
        builder.Property(wse => wse.ExerciseId).IsRequired();
        builder.Property(wse => wse.Order).IsRequired();
        builder.Property(wse => wse.IsCompleted).IsRequired();
        builder.Property(wse => wse.StartTime);
        builder.Property(wse => wse.EndTime);
        builder.Property(wse => wse.Notes).HasMaxLength(500);

        // Indexes
        builder.HasIndex(wse => wse.WorkoutSessionId);
        builder.HasIndex(wse => wse.ExerciseId);
        builder.HasIndex(wse => new { wse.WorkoutSessionId, wse.Order }).IsUnique();

        // Foreign key relationships
        builder.HasOne<WorkoutSession>()
            .WithMany()
            .HasForeignKey(wse => wse.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Exercise>()
            .WithMany()
            .HasForeignKey(wse => wse.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Sets collection is configured as owned type in DbContext
        builder.Ignore(wse => wse.Sets);
    }
}

// =====