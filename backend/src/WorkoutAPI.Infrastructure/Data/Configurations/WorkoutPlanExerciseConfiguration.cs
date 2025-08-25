using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class WorkoutPlanExerciseConfiguration : IEntityTypeConfiguration<WorkoutPlanExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutPlanExercise> builder)
    {
        builder.ToTable("WorkoutPlanExercises");

        builder.HasKey(wpe => wpe.Id);
        builder.Property(wpe => wpe.Id).ValueGeneratedNever();

        builder.Property(wpe => wpe.WorkoutPlanId).IsRequired();
        builder.Property(wpe => wpe.ExerciseId).IsRequired();
        builder.Property(wpe => wpe.Day).IsRequired();
        builder.Property(wpe => wpe.Order).IsRequired();
        builder.Property(wpe => wpe.Sets).IsRequired();
        builder.Property(wpe => wpe.Reps);
        builder.Property(wpe => wpe.Weight).HasColumnType("decimal(8,2)");
        builder.Property(wpe => wpe.Duration);
        builder.Property(wpe => wpe.RestTime);
        builder.Property(wpe => wpe.Notes).HasMaxLength(500);

        builder.HasIndex(wpe => wpe.WorkoutPlanId);
        builder.HasIndex(wpe => wpe.ExerciseId);
        builder.HasIndex(wpe => new { wpe.WorkoutPlanId, wpe.Day, wpe.Order }).IsUnique();

        builder.HasOne<WorkoutPlan>()
            .WithMany()
            .HasForeignKey(wpe => wpe.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Exercise>()
            .WithMany()
            .HasForeignKey(wpe => wpe.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
