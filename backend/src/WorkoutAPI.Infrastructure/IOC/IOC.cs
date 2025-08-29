
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WorkoutAPI.Domain.Events;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;
using WorkoutAPI.Infrastructure.Repositories;

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

        services.Scan(scan => scan
            .FromAssemblyOf<IUserRepository>() // or any known type in the same assembly
            .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        // Add MediatR for domain events
        services.AddMediatR(cfg =>
        {
            // Register from Domain assembly for events
            cfg.RegisterServicesFromAssembly(typeof(IDomainEvent).Assembly);
            // Register from Infrastructure assembly for handlers
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });



        // Generic repository
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        // Add Health Checks
        services.AddHealthChecks()
         .AddDbContextCheck<WorkoutDbContext>("database", failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded);


        return services;
    }
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Add MediatR for domain events
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(WorkoutAPI.Domain.Events.IDomainEvent).Assembly);
        });

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
