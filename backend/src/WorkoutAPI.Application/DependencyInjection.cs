using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WorkoutAPI.Application.Services;
using WorkoutAPI.Application.Validators;

namespace WorkoutAPI.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        return services;
    }
}