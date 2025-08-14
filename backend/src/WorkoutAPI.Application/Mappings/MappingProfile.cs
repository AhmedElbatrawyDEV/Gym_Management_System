using Mapster;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Application.Mappings;

public static class MappingProfile
{
    public static void Configure()
    {
        // User mappings
        TypeAdapterConfig<CreateUserRequest, User>
            .NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.IsActive, src => true)
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow);

        TypeAdapterConfig<User, UserResponse>
            .NewConfig()
            .Map(dest => dest.FullName, src => src.FullName)
            .Map(dest => dest.Age, src => src.Age);

        TypeAdapterConfig<User, UserProfileResponse>
            .NewConfig()
            .Map(dest => dest.FullName, src => src.FullName)
            .Map(dest => dest.Age, src => src.Age)
            .Map(dest => dest.WorkoutPlans, src => src.UserWorkoutPlans)
            .Map(dest => dest.RecentSessions, src => src.WorkoutSessions.Take(10));

        // Exercise mappings
        TypeAdapterConfig<Exercise, ExerciseResponse>
            .NewConfig()
            .Map(dest => dest.Name, src => GetTranslation(src.Translations, Language.Arabic).Name)
            .Map(dest => dest.Description, src => GetTranslation(src.Translations, Language.Arabic).Description)
            .Map(dest => dest.Instructions, src => GetTranslation(src.Translations, Language.Arabic).Instructions);

        // WorkoutSession mappings
        TypeAdapterConfig<WorkoutSession, WorkoutSessionResponse>
            .NewConfig()
            .Map(dest => dest.UserName, src => src.User.FullName)
            .Map(dest => dest.WorkoutPlanName, src => GetWorkoutPlanTranslation(src.WorkoutPlan.Translations, Language.Arabic).Name)
            .Map(dest => dest.WorkoutType, src => src.WorkoutPlan.Type);

        TypeAdapterConfig<WorkoutExerciseSession, WorkoutExerciseSessionResponse>
            .NewConfig()
            .Map(dest => dest.ExerciseName, src => GetTranslation(src.Exercise.Translations, Language.Arabic).Name)
            .Map(dest => dest.ExerciseDescription, src => GetTranslation(src.Exercise.Translations, Language.Arabic).Description)
            .Map(dest => dest.ExerciseInstructions, src => GetTranslation(src.Exercise.Translations, Language.Arabic).Instructions)
            .Map(dest => dest.ExerciseIcon, src => src.Exercise.IconName);

        // ExerciseSetRecord mappings
        TypeAdapterConfig<ExerciseSetRecord, ExerciseSetRecordResponse>
            .NewConfig();

        // UserWorkoutPlan mappings
        TypeAdapterConfig<UserWorkoutPlan, UserWorkoutPlanResponse>
            .NewConfig()
            .Map(dest => dest.PlanName, src => GetWorkoutPlanTranslation(src.WorkoutPlan.Translations, Language.Arabic).Name)
            .Map(dest => dest.Type, src => src.WorkoutPlan.Type);

        // WorkoutSession summary mappings
        TypeAdapterConfig<WorkoutSession, WorkoutSessionSummaryResponse>
            .NewConfig()
            .Map(dest => dest.WorkoutPlanName, src => GetWorkoutPlanTranslation(src.WorkoutPlan.Translations, Language.Arabic).Name)
            .Map(dest => dest.Type, src => src.WorkoutPlan.Type);
    }

    private static ExerciseTranslation GetTranslation(ICollection<ExerciseTranslation> translations, Language language)
    {
        return translations.FirstOrDefault(t => t.Language == language) 
               ?? translations.FirstOrDefault() 
               ?? new ExerciseTranslation { Name = "Unknown", Description = null, Instructions = null };
    }

    private static WorkoutPlanTranslation GetWorkoutPlanTranslation(ICollection<WorkoutPlanTranslation> translations, Language language)
    {
        return translations.FirstOrDefault(t => t.Language == language) 
               ?? translations.FirstOrDefault() 
               ?? new WorkoutPlanTranslation { Name = "Unknown", Description = null, Goals = null };
    }
}

