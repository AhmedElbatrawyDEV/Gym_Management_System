using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WorkoutAPI.Application.Common.Behaviors;
using WorkoutAPI.Application.Common.Interfaces;
using WorkoutAPI.Application.Common.Mappings;

namespace WorkoutAPI.Application.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Register Mapster
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        // Register all handlers using Scrutor
        services.Scan(scan => scan
            .FromAssemblyOf<ICurrentUserService>()
            .AddClasses(classes => classes
                .AssignableTo(typeof(IRequestHandler<,>))
                .Where(type => type.Name.EndsWith("Handler")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Register behaviors
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

        // Register validators using Scrutor
        services.Scan(scan => scan
            .FromAssemblyOf<ICurrentUserService>()
            .AddClasses(classes => classes
                .AssignableTo(typeof(FluentValidation.IValidator<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
