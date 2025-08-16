using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using WorkoutAPI.Application.Mappings;
using WorkoutAPI.Application.Services;
using WorkoutAPI.Application.Validators;

namespace WorkoutAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Configure Mapster
        MappingProfile.Configure();

        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

        // Add Application Services
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}

