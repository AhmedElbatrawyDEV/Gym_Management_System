using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Infrastructure.Data.Configurations;

namespace WorkoutAPI.Infrastructure.Data;

public class WorkoutDbContext : DbContext
{
    public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) { }

    // Aggregate Roots
    public DbSet<User> Users { get; set; }
    public DbSet<WorkoutSession> WorkoutSessions { get; set; }
    public DbSet<Payment> Payments { get; set; }

    // Entities
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<WorkoutSessionExercise> WorkoutSessionExercises { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseTranslation> ExerciseTranslations { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    // Missing DbSets that you have entity configurations for
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<ClassBooking> ClassBookings { get; set; }
    public DbSet<GymClass> GymClasses { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Translation> Translations { get; set; }
    public DbSet<ClassSchedule> ClassSchedules { get; set; }
    public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
    public DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }
    public DbSet<UserWorkoutPlan> UserWorkoutPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new WorkoutSessionConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new UserSubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new WorkoutSessionExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new ExerciseTranslationConfiguration());
        modelBuilder.ApplyConfiguration(new TrainerConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionPlanConfiguration());

        // Apply missing configurations
        modelBuilder.ApplyConfiguration(new AttendanceRecordConfiguration());
        modelBuilder.ApplyConfiguration(new ClassBookingConfiguration());
        modelBuilder.ApplyConfiguration(new GymClassConfiguration());
        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new MemberConfiguration());
        modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new TranslationConfiguration());
        modelBuilder.ApplyConfiguration(new ClassScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new WorkoutPlanConfiguration());
        modelBuilder.ApplyConfiguration(new WorkoutPlanExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new UserWorkoutPlanConfiguration());

        // Configure value objects
        ConfigureValueObjects(modelBuilder);
    }

    private void ConfigureValueObjects(ModelBuilder modelBuilder)
    {
        // Configure PersonalInfo value object
        modelBuilder.Entity<User>()
            .OwnsOne(u => u.PersonalInfo, pi =>
            {
                pi.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
                pi.Property(p => p.LastName).HasMaxLength(50).IsRequired();
                pi.Property(p => p.DateOfBirth).IsRequired();
                pi.Property(p => p.Gender).IsRequired();
            });

        // Configure ContactInfo value object
        modelBuilder.Entity<User>()
            .OwnsOne(u => u.ContactInfo, ci =>
            {
                ci.Property(c => c.Email).HasMaxLength(255).IsRequired();
                ci.Property(c => c.PhoneNumber).HasMaxLength(20);
                ci.HasIndex(c => c.Email).IsUnique();
            });

        // Configure Money value object
        modelBuilder.Entity<Payment>()
            .OwnsOne(p => p.Amount, m =>
            {
                m.Property(mo => mo.Amount).HasColumnType("decimal(18,2)").IsRequired();
                m.Property(mo => mo.Currency).HasMaxLength(3).IsRequired();
            });

        modelBuilder.Entity<SubscriptionPlan>()
            .OwnsOne(sp => sp.Price, m =>
            {
                m.Property(mo => mo.Amount).HasColumnType("decimal(18,2)").IsRequired();
                m.Property(mo => mo.Currency).HasMaxLength(3).IsRequired();
            });

        modelBuilder.Entity<Trainer>()
            .OwnsOne(t => t.HourlyRate, m =>
            {
                m.Property(mo => mo.Amount).HasColumnType("decimal(18,2)").IsRequired();
                m.Property(mo => mo.Currency).HasMaxLength(3).IsRequired();
            });

        // Configure DateRange value object
        modelBuilder.Entity<UserSubscription>()
            .OwnsOne(us => us.Period, dr =>
            {
                dr.Property(d => d.StartDate).IsRequired();
                dr.Property(d => d.EndDate).IsRequired();
            });

        // Configure ExerciseSetRecord value object collection
        modelBuilder.Entity<WorkoutSessionExercise>()
            .OwnsMany(wse => wse.Sets, set =>
            {
                set.Property(s => s.SetNumber).IsRequired();
                set.Property(s => s.Reps);
                set.Property(s => s.Weight).HasColumnType("decimal(8,2)");
                set.Property(s => s.Distance);
                set.WithOwner().HasForeignKey("WorkoutSessionExerciseId");
            });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is IAggregateRoot aggregateRoot)
            {
                if (entry.State == EntityState.Added)
                {
                    // Handle creation timestamp if needed
                }
                // Handle update timestamp if needed
            }
        }
    }
}