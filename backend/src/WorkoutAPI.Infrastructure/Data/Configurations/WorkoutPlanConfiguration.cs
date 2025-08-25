using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
    {
        builder.ToTable("WorkoutPlans");

        builder.HasKey(wp => wp.Id);
        builder.Property(wp => wp.Id).ValueGeneratedNever();

        builder.Property(wp => wp.Name).HasMaxLength(100).IsRequired();
        builder.Property(wp => wp.Description).HasMaxLength(500);
        builder.Property(wp => wp.Type).IsRequired();
        builder.Property(wp => wp.DifficultyLevel).IsRequired();
        builder.Property(wp => wp.DurationWeeks).IsRequired();
        builder.Property(wp => wp.CreatedBy).IsRequired();
        builder.Property(wp => wp.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(wp => wp.CreatedAt).IsRequired();

        builder.HasIndex(wp => wp.Name);
        builder.HasIndex(wp => wp.Type);
        builder.HasIndex(wp => wp.DifficultyLevel);
        builder.HasIndex(wp => wp.CreatedBy);
        builder.HasIndex(wp => wp.IsActive);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(wp => wp.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
