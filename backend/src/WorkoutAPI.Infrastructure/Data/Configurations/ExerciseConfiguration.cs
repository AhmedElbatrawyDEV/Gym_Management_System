using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise> {
    public void Configure(EntityTypeBuilder<Exercise> builder) {
        builder.ToTable("Exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.IconName)
            .HasMaxLength(100);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(255);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(255);

        // Relationships
        builder.HasMany(e => e.Translations)
            .WithOne(et => et.Exercise)
            .HasForeignKey(et => et.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.WorkoutPlanExercises)
            .WithOne(wpe => wpe.Exercise)
            .HasForeignKey(wpe => wpe.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.WorkoutExerciseSessions)
            .WithOne(wes => wes.Exercise)
            .HasForeignKey(wes => wes.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

