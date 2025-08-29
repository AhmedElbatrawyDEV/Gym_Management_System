using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class UserWorkoutPlanConfiguration : IEntityTypeConfiguration<UserWorkoutPlan>
{
    public void Configure(EntityTypeBuilder<UserWorkoutPlan> builder)
    {
        builder.ToTable("UserWorkoutPlans");

        builder.HasKey(uwp => uwp.Id);
        builder.Property(uwp => uwp.Id).ValueGeneratedNever();

        builder.Property(uwp => uwp.UserId).IsRequired();
        builder.Property(uwp => uwp.WorkoutPlanId).IsRequired();
        builder.Property(uwp => uwp.StartDate).IsRequired();
        builder.Property(uwp => uwp.EndDate);
        builder.Property(uwp => uwp.Status).IsRequired();
        builder.Property(uwp => uwp.Progress).HasColumnType("decimal(5,2)").HasDefaultValue(0);
        builder.Property(uwp => uwp.AssignedBy).IsRequired();
        builder.Property(uwp => uwp.CreatedAt).IsRequired();

        builder.HasIndex(uwp => uwp.UserId);
        builder.HasIndex(uwp => uwp.WorkoutPlanId);
        builder.HasIndex(uwp => uwp.Status);
        builder.HasIndex(uwp => uwp.AssignedBy);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(uwp => uwp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<WorkoutPlan>()
            .WithMany()
            .HasForeignKey(uwp => uwp.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(uwp => uwp.AssignedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
