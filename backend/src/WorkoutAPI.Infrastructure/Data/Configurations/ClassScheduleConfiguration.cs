using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class ClassScheduleConfiguration : IEntityTypeConfiguration<ClassSchedule>
{
    public void Configure(EntityTypeBuilder<ClassSchedule> builder)
    {
        builder.ToTable("ClassSchedules");

        builder.HasKey(cs => cs.Id);
        builder.Property(cs => cs.Id).ValueGeneratedNever();

        builder.Property(cs => cs.GymClassId).IsRequired();
        builder.Property(cs => cs.StartTime).IsRequired();
        builder.Property(cs => cs.EndTime).IsRequired();
        builder.Property(cs => cs.MaxCapacity).IsRequired();
        builder.Property(cs => cs.CurrentEnrollment).IsRequired().HasDefaultValue(0);
        builder.Property(cs => cs.Status).IsRequired();

        builder.HasIndex(cs => cs.GymClassId);
        builder.HasIndex(cs => cs.StartTime);
        builder.HasIndex(cs => cs.Status);

        builder.HasOne<GymClass>()
            .WithMany()
            .HasForeignKey(cs => cs.GymClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<ClassBooking>()
            .WithOne()
            .HasForeignKey(cb => cb.ClassScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
