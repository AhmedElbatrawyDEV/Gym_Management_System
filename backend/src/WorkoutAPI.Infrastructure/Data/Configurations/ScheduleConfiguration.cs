using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule> {
    public void Configure(EntityTypeBuilder<Schedule> builder) {
        builder.ToTable("Schedules");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.CreatedBy)
            .HasMaxLength(255);

        builder.Property(s => s.UpdatedBy)
            .HasMaxLength(255);

        // Relationships
        builder.HasOne(s => s.Trainer)
            .WithMany()
            .HasForeignKey(s => s.TrainerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(s => s.WorkoutPlan)
            .WithMany()
            .HasForeignKey(s => s.WorkoutPlanId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

