using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Infrastructure.Data;

public class WorkoutDbContext : DbContext {
    public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseTranslation> ExerciseTranslations { get; set; }
    public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
    public DbSet<WorkoutPlanTranslation> WorkoutPlanTranslations { get; set; }
    public DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }
    public DbSet<UserWorkoutPlan> UserWorkoutPlans { get; set; }
    public DbSet<WorkoutSession> WorkoutSessions { get; set; }
    public DbSet<WorkoutExerciseSession> WorkoutExerciseSessions { get; set; }
    public DbSet<ExerciseSetRecord> ExerciseSetRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkoutDbContext).Assembly);

        // Configure enums to be stored as strings
        ConfigureEnums(modelBuilder);

        // Configure indexes
        ConfigureIndexes(modelBuilder);
    }

    private static void ConfigureEnums(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>()
            .Property(e => e.Gender)
            .HasConversion<string>();

        modelBuilder.Entity<Exercise>()
            .Property(e => e.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Exercise>()
            .Property(e => e.PrimaryMuscleGroup)
            .HasConversion<string>();

        modelBuilder.Entity<Exercise>()
            .Property(e => e.SecondaryMuscleGroup)
            .HasConversion<string>();

        modelBuilder.Entity<Exercise>()
            .Property(e => e.Difficulty)
            .HasConversion<string>();

        modelBuilder.Entity<WorkoutPlan>()
            .Property(e => e.Type)
            .HasConversion<string>();

        modelBuilder.Entity<WorkoutSession>()
            .Property(e => e.Status)
            .HasConversion<string>();

        modelBuilder.Entity<ExerciseTranslation>()
            .Property(e => e.Language)
            .HasConversion<string>();

        modelBuilder.Entity<WorkoutPlanTranslation>()
            .Property(e => e.Language)
            .HasConversion<string>();

    }

    private static void ConfigureIndexes(ModelBuilder modelBuilder) {
        // User indexes
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        // Exercise indexes
        modelBuilder.Entity<Exercise>()
            .HasIndex(e => e.Code)
            .IsUnique();

        modelBuilder.Entity<Exercise>()
            .HasIndex(e => e.Type);

        // WorkoutPlan indexes
        modelBuilder.Entity<WorkoutPlan>()
            .HasIndex(wp => wp.Code)
            .IsUnique();

        modelBuilder.Entity<WorkoutPlan>()
            .HasIndex(wp => wp.Type);

        // Translation indexes
        modelBuilder.Entity<ExerciseTranslation>()
            .HasIndex(et => new { et.ExerciseId, et.Language })
            .IsUnique();

        modelBuilder.Entity<WorkoutPlanTranslation>()
            .HasIndex(wpt => new { wpt.WorkoutPlanId, wpt.Language })
            .IsUnique();

        // Session indexes
        modelBuilder.Entity<WorkoutSession>()
            .HasIndex(ws => new { ws.UserId, ws.Status });

        modelBuilder.Entity<WorkoutSession>()
            .HasIndex(ws => ws.StartTime);
    }
}

