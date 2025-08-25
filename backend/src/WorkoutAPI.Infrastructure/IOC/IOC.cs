
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkoutAPI.Domain.Common;
using WorkoutAPI.Infrastructure.Repositories;
using System.Data.Common;
using MediatR;
using System.Reflection;
using WorkoutAPI.Infrastructure.Data;
using WorkoutAPI.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using WorkoutAPI.Domain.Events;
using System.Data.Common;
using WorkoutAPI.Infrastructure.EventHandlers;

namespace WorkoutAPI.Infrastructure.IOC;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<WorkoutDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(WorkoutDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(30);
            });

            // Enable sensitive data logging in development
            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // Add Unit of Work
        services.AddScoped<IUnitOfWork>(provider =>
        {
            var context = provider.GetRequiredService<WorkoutDbContext>();
            var connection = context.Database.GetDbConnection();
            var mediator = provider.GetRequiredService<IMediator>();

            var unitOfWork = new UnitOfWork(connection, mediator, CancellationToken.None);
            unitOfWork.RegisterContext(context);

            return unitOfWork;
        });

        // Add Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkoutSessionRepository, WorkoutSessionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();

        // Add generic repository for entities not having specific repositories
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        // Add MediatR for domain events
        services.AddMediatR(cfg =>
        {
            // Register from Domain assembly for events
            cfg.RegisterServicesFromAssembly(typeof(IDomainEvent).Assembly);
            // Register from Infrastructure assembly for handlers
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        // Add Domain Event Handlers
        services.AddScoped<INotificationHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
        services.AddScoped<INotificationHandler<WorkoutSessionCompletedEvent>, WorkoutSessionCompletedEventHandler>();
        services.AddScoped<INotificationHandler<PaymentProcessedEvent>, PaymentProcessedEventHandler>();

        // Add Health Checks
        services.AddHealthChecks()
         .AddDbContextCheck<WorkoutDbContext>("database", failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded);


        return services;
    }

    public static IServiceCollection AddInfrastructureOptions(this IServiceCollection services,
       IConfiguration configuration)
    {
        // Fix: Use Bind to map the configuration section to the DatabaseOptions object  
        services.Configure<DatabaseOptions>(options =>
            configuration.GetSection(DatabaseOptions.SectionName).Bind(options));

        return services;
    }
}

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public string ConnectionString { get; set; } = string.Empty;
    public int CommandTimeout { get; set; } = 30;
    public int MaxRetryCount { get; set; } = 3;
    public int MaxRetryDelay { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public bool EnableDetailedErrors { get; set; } = false;
    public bool EnableQuerySplitting { get; set; } = true;
}