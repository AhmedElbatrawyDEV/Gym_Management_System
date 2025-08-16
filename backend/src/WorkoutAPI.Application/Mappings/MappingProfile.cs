using AutoMapper;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;

namespace WorkoutAPI.Application;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName));

        CreateMap<Exercise, ExerciseResponse>();

        CreateMap<WorkoutPlan, UserWorkoutPlanResponse>()
            .ForMember(d => d.PlanId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.ExerciseCount, opt => opt.MapFrom(s => s.Exercises.Count));

        CreateMap<WorkoutPlan, WorkoutSessionSummaryResponse>()
            .ForMember(d => d.PlanId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.TotalExercises, opt => opt.MapFrom(s => s.Exercises.Count))
            .ForMember(d => d.TotalSets, opt => opt.MapFrom(s => s.Exercises.Sum(e => e.Sets)))
            .ForMember(d => d.TotalReps, opt => opt.MapFrom(s => s.Exercises.Sum(e => e.Reps)));
    }
}