using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        // Table configuration
        builder.ToTable("Trainers");

        // Primary key
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        // Properties
        builder.Property(t => t.UserId).IsRequired();
        builder.Property(t => t.Specialization).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Certification).HasMaxLength(500).IsRequired();
        builder.Property(t => t.IsAvailable).IsRequired().HasDefaultValue(true);

        // Indexes
        builder.HasIndex(t => t.UserId).IsUnique();
        builder.HasIndex(t => t.Specialization);
        builder.HasIndex(t => t.IsAvailable);

        // Foreign key relationships
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// =====