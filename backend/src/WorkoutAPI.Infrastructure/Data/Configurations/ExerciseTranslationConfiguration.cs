using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class ExerciseTranslationConfiguration : IEntityTypeConfiguration<ExerciseTranslation>
{
    public void Configure(EntityTypeBuilder<ExerciseTranslation> builder)
    {
        // Table configuration
        builder.ToTable("ExerciseTranslations");

        // Primary key
        builder.HasKey(et => et.Id);
        builder.Property(et => et.Id).ValueGeneratedNever();

        // Properties
        builder.Property(et => et.ExerciseId).IsRequired();
        builder.Property(et => et.Language).IsRequired();
        builder.Property(et => et.Name).HasMaxLength(200).IsRequired();
        builder.Property(et => et.Description).HasMaxLength(1000);
        builder.Property(et => et.Instructions).HasMaxLength(2000);

        // Indexes
        builder.HasIndex(et => et.ExerciseId);
        builder.HasIndex(et => et.Language);
        builder.HasIndex(et => new { et.ExerciseId, et.Language }).IsUnique();

        // Foreign key relationships
        builder.HasOne<Exercise>()
            .WithMany()
            .HasForeignKey(et => et.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// =====