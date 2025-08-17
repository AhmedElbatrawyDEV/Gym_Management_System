// Infrastructure/Data/WorkoutDbContext.cs
using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using System.Text.Json;
using WorkoutAPI.Domain.Entities.WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Infrastructure.Data
{
    public class WorkoutDbContext : DbContext
    {
        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options)
        {
        }

        // Existing entities
        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseTranslation> ExerciseTranslations { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }
        public DbSet<WorkoutPlanTranslation> WorkoutPlanTranslations { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<WorkoutSessionExercise> WorkoutSessionExercises { get; set; }

        // New entities (DO NOT include ExerciseSetRecord as DbSet)
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<GymClass> GymClasses { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<ClassBooking> ClassBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure existing entities
            ConfigureUserEntity(modelBuilder);
            ConfigureExerciseEntities(modelBuilder);
            ConfigureWorkoutEntities(modelBuilder);

            // Configure new entities
            ConfigureAdminEntity(modelBuilder);
            ConfigureSubscriptionEntities(modelBuilder);
            ConfigurePaymentEntities(modelBuilder);
            ConfigureAttendanceEntities(modelBuilder);
            ConfigureGymClassEntities(modelBuilder);

            // Seed default data
            SeedDefaultData(modelBuilder);
        }

        private static void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.MembershipNumber).HasMaxLength(20);
                entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);
                entity.Property(e => e.Gender).HasConversion<int>();
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.Language).HasConversion<string>().HasMaxLength(10);
            });
        }

        private static void ConfigureAdminEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Role).HasConversion<int>();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.FailedLoginAttempts).HasDefaultValue(0);
            });
        }

        private static void ConfigureSubscriptionEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Configure Features as JSON
                entity.Property(e => e.Features)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
                    )
                    .HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.AutoRenew).HasDefaultValue(false);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.SubscriptionPlan)
                    .WithMany(p => p.UserSubscriptions)
                    .HasForeignKey(e => e.SubscriptionPlanId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigurePaymentEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(3).HasDefaultValue("SAR");
                entity.Property(e => e.PaymentMethod).HasConversion<int>();
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.TransactionId).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);

                // Configure Metadata as JSON
                entity.Property(e => e.Metadata)
                    .HasConversion(
                        v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => v == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null)
                    )
                    .HasColumnType("nvarchar(max)");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.UserSubscription)
                    .WithMany(s => s.Payments)
                    .HasForeignKey(e => e.UserSubscriptionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasConversion<int>();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Payment)
                    .WithOne(p => p.Invoice)
                    .HasForeignKey<Invoice>(e => e.PaymentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void ConfigureAttendanceEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ActivityType).HasConversion<int>();
                entity.Property(e => e.CheckInTime).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.CheckInTime });
            });
        }

        private static void ConfigureGymClassEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GymClass>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.MaxCapacity).IsRequired();
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.Difficulty).HasConversion<int>();
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.Instructor)
                    .WithMany()
                    .HasForeignKey(e => e.InstructorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ClassSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.GymClass)
                    .WithMany(c => c.Schedules)
                    .HasForeignKey(e => e.GymClassId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.GymClassId, e.StartTime });
            });

            modelBuilder.Entity<ClassBooking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BookingDate).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ClassSchedule)
                    .WithMany(s => s.Bookings)
                    .HasForeignKey(e => e.ClassScheduleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.ClassScheduleId }).IsUnique();
            });
        }

        private static void ConfigureExerciseEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Type).HasConversion<int>();
                entity.Property(e => e.PrimaryMuscleGroup).HasConversion<int>();
                entity.Property(e => e.SecondaryMuscleGroup).HasConversion<int>();
                entity.Property(e => e.Difficulty).HasConversion<int>();
                entity.Property(e => e.IconName).HasMaxLength(100);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<ExerciseTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Language).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Instructions).HasMaxLength(2000);

                entity.HasOne(e => e.Exercise)
                    .WithMany(ex => ex.Translations)
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.ExerciseId, e.Language }).IsUnique();
            });
        }

        private static void ConfigureWorkoutEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkoutPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Type).HasConversion<int>();
                entity.Property(e => e.Difficulty).HasConversion<int>();
                entity.Property(e => e.EstimatedDurationMinutes).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<WorkoutPlanTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Language).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.HasOne(e => e.WorkoutPlan)
                    .WithMany(wp => wp.Translations)
                    .HasForeignKey(e => e.WorkoutPlanId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.WorkoutPlanId, e.Language }).IsUnique();
            });

            modelBuilder.Entity<WorkoutPlanExercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Order).IsRequired();
                entity.Property(e => e.Sets).IsRequired();
                entity.Property(e => e.Reps).HasMaxLength(20);
                entity.Property(e => e.Weight).HasMaxLength(20);
                entity.Property(e => e.RestSeconds).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(500);

                //entity.HasOne(e => e.WorkoutPlan)
                //    .WithMany(wp => wp.Exercis)
                //    .HasForeignKey(e => e.WorkoutPlanId)
                //    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Exercise)
                    .WithMany()
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.WorkoutPlanId, e.Order }).IsUnique();
            });

            modelBuilder.Entity<WorkoutSession>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.WorkoutPlan)
                    .WithMany()
                    .HasForeignKey(e => e.WorkoutPlanId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.UserId, e.StartTime });
            });

            modelBuilder.Entity<WorkoutSessionExercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Order).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.HasOne(e => e.WorkoutSession)
                    .WithMany(ws => ws.Exercises)
                    .HasForeignKey(e => e.WorkoutSessionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Exercise)
                    .WithMany()
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure ExerciseSetRecord as owned entity type
                entity.OwnsMany(e => e.Sets, setBuilder =>
                {
                    setBuilder.Property(s => s.SetNumber).IsRequired();
                    setBuilder.Property(s => s.Reps).IsRequired();
                    setBuilder.Property(s => s.Weight).HasColumnType("decimal(10,2)");
                    setBuilder.Property(s => s.RestSeconds).IsRequired();
                    setBuilder.Property(s => s.CompletedAt).IsRequired();
                    setBuilder.Property(s => s.Notes).HasMaxLength(200);
                });
            });
        }

        private static void SeedDefaultData(ModelBuilder modelBuilder)
        {
            // Seed default admin
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            modelBuilder.Entity<Admin>().HasData(new Admin
            {
                Id = adminId,
                FirstName = "System",
                LastName = "Administrator",
                Email = "admin@gym.com",
                PasswordHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // "admin123"
                Role = AdminRole.SuperAdmin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // Seed subscription plans
            var basicPlanId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var standardPlanId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var premiumPlanId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            modelBuilder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan
                {
                    Id = basicPlanId,
                    Name = "Basic Plan",
                    Description = "Access to gym equipment during regular hours",
                    Price = 150.00m,
                    DurationDays = 30,
                    Features = JsonSerializer.Serialize(new List<string> { "Gym Access", "Basic Equipment", "Locker Room" }),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SubscriptionPlan
                {
                    Id = standardPlanId,
                    Name = "Standard Plan",
                    Description = "Full gym access with group classes",
                    Price = 250.00m,
                    DurationDays = 30,
                    Features = JsonSerializer.Serialize(new List<string> { "Full Gym Access", "Group Classes", "Locker Room", "Nutrition Consultation" }),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SubscriptionPlan
                {
                    Id = premiumPlanId,
                    Name = "Premium Plan",
                    Description = "All-inclusive membership with personal training",
                    Price = 450.00m,
                    DurationDays = 30,
                    Features = JsonSerializer.Serialize(new List<string> { "24/7 Access", "Personal Training", "Group Classes", "Nutrition Plan", "Massage Therapy" }),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed default gym classes
            var yogaClassId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var crossfitClassId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            modelBuilder.Entity<GymClass>().HasData(
                new GymClass
                {
                    Id = yogaClassId,
                    Name = "Morning Yoga",
                    Description = "Relaxing yoga session to start your day",
                    MaxCapacity = 20,
                    Duration = TimeSpan.FromMinutes(60),
                    Difficulty = DifficultyLevel.Beginner,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new GymClass
                {
                    Id = crossfitClassId,
                    Name = "CrossFit Challenge",
                    Description = "High-intensity functional fitness workout",
                    MaxCapacity = 15,
                    Duration = TimeSpan.FromMinutes(45),
                    Difficulty = DifficultyLevel.Advanced,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}