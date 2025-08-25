using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class ClassBookingConfiguration : IEntityTypeConfiguration<ClassBooking>
{
    public void Configure(EntityTypeBuilder<ClassBooking> builder)
    {
        builder.ToTable("ClassBookings");

        builder.HasKey(cb => cb.Id);
        builder.Property(cb => cb.Id).ValueGeneratedNever();

        builder.Property(cb => cb.UserId).IsRequired();
        builder.Property(cb => cb.ClassScheduleId).IsRequired();
        builder.Property(cb => cb.BookingDate).IsRequired();
        builder.Property(cb => cb.Status).IsRequired();

        builder.HasIndex(cb => cb.UserId);
        builder.HasIndex(cb => cb.ClassScheduleId);
        builder.HasIndex(cb => cb.BookingDate);
        builder.HasIndex(cb => cb.Status);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(cb => cb.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// =====