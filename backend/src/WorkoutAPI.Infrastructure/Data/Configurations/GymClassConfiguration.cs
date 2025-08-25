using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class GymClassConfiguration : IEntityTypeConfiguration<GymClass>
{
    public void Configure(EntityTypeBuilder<GymClass> builder)
    {
        builder.ToTable("GymClasses");

        builder.HasKey(gc => gc.Id);
        builder.Property(gc => gc.Id).ValueGeneratedNever();

        builder.Property(gc => gc.Name).HasMaxLength(100).IsRequired();
        builder.Property(gc => gc.Description).HasMaxLength(500).IsRequired();
        builder.Property(gc => gc.InstructorId);
        builder.Property(gc => gc.MaxCapacity).IsRequired();
        builder.Property(gc => gc.Duration).IsRequired();
        builder.Property(gc => gc.Difficulty).IsRequired();
        builder.Property(gc => gc.IsActive).IsRequired().HasDefaultValue(true);

        builder.HasIndex(gc => gc.Name);
        builder.HasIndex(gc => gc.InstructorId);
        builder.HasIndex(gc => gc.Difficulty);
        builder.HasIndex(gc => gc.IsActive);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(gc => gc.InstructorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

// =====