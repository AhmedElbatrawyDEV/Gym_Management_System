using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member> {
    public void Configure(EntityTypeBuilder<Member> builder) {
        builder.ToTable("Members");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.CreatedBy)
            .HasMaxLength(255);

        builder.Property(m => m.UpdatedBy)
            .HasMaxLength(255);

        // Relationships
        builder.HasOne(m => m.User)
            .WithOne()
            .HasForeignKey<Member>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.AttendedSessions)
            .WithOne(ws => ws.Member)
            .HasForeignKey(ws => ws.MemberId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(m => m.EnrolledWorkoutPlans)
            .WithOne(uwp => uwp.Member)
            .HasForeignKey(uwp => uwp.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

