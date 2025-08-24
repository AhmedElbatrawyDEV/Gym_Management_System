using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class ExerciseTranslationConfiguration : IEntityTypeConfiguration<ExerciseTranslation> {
    public void Configure(EntityTypeBuilder<ExerciseTranslation> builder) {
        builder.ToTable("ExerciseTranslations");

        builder.HasKey(et => et.Id);

        builder.Property(et => et.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(et => et.Description)
            .HasMaxLength(1000);

        builder.Property(et => et.Instructions)
            .HasMaxLength(2000);

        builder.Property(et => et.CreatedBy)
            .HasMaxLength(255);

        builder.Property(et => et.UpdatedBy)
            .HasMaxLength(255);

        // Relationships
        builder.HasOne(et => et.Exercise)
            .WithMany(e => e.Translations)
            .HasForeignKey(et => et.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

