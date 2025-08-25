using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

// ===== ADDITIONAL ENTITY CONFIGURATIONS =====

public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("AttendanceRecords");

        builder.HasKey(ar => ar.Id);
        builder.Property(ar => ar.Id).ValueGeneratedNever();

        builder.Property(ar => ar.UserId).IsRequired();
        builder.Property(ar => ar.CheckInTime).IsRequired();
        builder.Property(ar => ar.CheckOutTime);
        builder.Property(ar => ar.DurationMinutes);
        builder.Property(ar => ar.ActivityType).IsRequired();

        builder.HasIndex(ar => ar.UserId);
        builder.HasIndex(ar => ar.CheckInTime);
        builder.HasIndex(ar => ar.ActivityType);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ar => ar.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// =====