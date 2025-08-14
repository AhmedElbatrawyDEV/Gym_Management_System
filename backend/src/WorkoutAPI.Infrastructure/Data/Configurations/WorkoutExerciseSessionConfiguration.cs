using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data.Configurations;

public class WorkoutExerciseSessionConfiguration
    : IEntityTypeConfiguration<WorkoutExerciseSession> {
    public void Configure(EntityTypeBuilder<WorkoutExerciseSession> builder) {
        // Configure owned collection for Sets
        builder.OwnsMany(
            e => e.Sets,
            b => {
                b.ToTable("ExerciseSetRecords"); // Optional: custom table name
                b.WithOwner().HasForeignKey("WorkoutExerciseSessionId");
                b.Property<int>("Id"); // Shadow property for primary key
                b.HasKey("Id"); // Composite key: Owner FK + this shadow ID

                // Map properties
                b.Property(s => s.SetNumber);
                b.Property(s => s.ActualReps);
                b.Property(s => s.ActualWeight);
                b.Property(s => s.ActualRestTime);
                b.Property(s => s.CompletedAt);
                b.Property(s => s.Notes);
            }
        );
    }
}

