using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        // Table configuration
        builder.ToTable("Exercises");

        // Primary key
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        // Properties
        builder.Property(e => e.Code).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.PrimaryMuscleGroup).IsRequired();
        builder.Property(e => e.SecondaryMuscleGroup);
        builder.Property(e => e.Difficulty).IsRequired();
        builder.Property(e => e.IconName).HasMaxLength(100);
        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);

        // Indexes
        builder.HasIndex(e => e.Code).IsUnique();
        builder.HasIndex(e => e.Type);
        builder.HasIndex(e => e.PrimaryMuscleGroup);
        builder.HasIndex(e => e.Difficulty);
        builder.HasIndex(e => e.IsActive);

        // Navigation properties
        builder.HasMany<ExerciseTranslation>()
            .WithOne()
            .HasForeignKey(et => et.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore collections that are handled via navigation
        builder.Ignore(e => e.Translations);
    }
}

// =====