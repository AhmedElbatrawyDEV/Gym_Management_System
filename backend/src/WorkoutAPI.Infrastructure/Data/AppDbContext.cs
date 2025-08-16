using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<MemberProfile> MemberProfiles => Set<MemberProfile>();
    public DbSet<TrainerProfile> TrainerProfiles => Set<TrainerProfile>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<GymClass> GymClasses => Set<GymClass>();
    public DbSet<ClassBooking> ClassBookings => Set<ClassBooking>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Translation> Translations => Set<Translation>();

public class WorkoutDbContext : DbContext {
    public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserCredentials> UserCredentials { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
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

        // Apply global query filter for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
            }
        }

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkoutDbContext).Assembly);

        // Configure enums
        ConfigureEnums(modelBuilder);

        // Configure indexes
        ConfigureIndexes(modelBuilder);
    }

    private static void ConfigureEnums(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(e => e.Gender)
            .HasConversion<string>();

        modelBuilder.Entity<UserCredentials>()
            .Property(e => e.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Member>()
            .Property(e => e.MembershipType)
            .HasConversion<string>();

        modelBuilder.Entity<Payment>()
            .Property(e => e.Status)
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

    private static void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        // User indexes
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        // UserCredentials indexes
        modelBuilder.Entity<UserCredentials>()
            .HasIndex(uc => uc.UserId)
            .IsUnique();

        modelBuilder.Entity<UserCredentials>()
            .HasIndex(uc => uc.Role);

        // Trainer indexes
        modelBuilder.Entity<Trainer>()
            .HasIndex(t => t.UserId)
            .IsUnique();

        modelBuilder.Entity<Trainer>()
            .HasIndex(t => t.Specialization);

        // Member indexes
        modelBuilder.Entity<Member>()
            .HasIndex(m => m.UserId)
            .IsUnique();

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.MembershipType);

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.MembershipEndDate);

        // Payment indexes
        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.MemberId);

        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.PaymentDate);

        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.Status);

        // Schedule indexes
        modelBuilder.Entity<Schedule>()
            .HasIndex(s => s.TrainerId);

        modelBuilder.Entity<Schedule>()
            .HasIndex(s => s.StartTime);

        modelBuilder.Entity<Schedule>()
            .HasIndex(s => s.WorkoutPlanId);

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



