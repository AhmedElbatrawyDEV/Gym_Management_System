using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.Property(s => s.Title).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(1000).IsRequired();
        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.EndTime).IsRequired();
        builder.Property(s => s.TrainerId);
        builder.Property(s => s.WorkoutPlanId);
        builder.Property(s => s.Capacity).IsRequired();
        builder.Property(s => s.EnrolledCount).IsRequired().HasDefaultValue(0);

        builder.HasIndex(s => s.StartTime);
        builder.HasIndex(s => s.TrainerId);
        builder.HasIndex(s => s.WorkoutPlanId);

        builder.HasOne<Trainer>()
            .WithMany()
            .HasForeignKey(s => s.TrainerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

// =====