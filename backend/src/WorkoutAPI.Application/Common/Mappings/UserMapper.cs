using Mapster;
using WorkoutAPI.Application.Features.Users.DTOs;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Application.Common.Mappings;

public class UserMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // User → UserDto
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.FirstName, src => src.PersonalInfo.FirstName)
                .Map(dest => dest.LastName, src => src.PersonalInfo.LastName)
                .Map(dest => dest.FullName, src => $"{src.PersonalInfo.FirstName} {src.PersonalInfo.LastName}")
                .Map(dest => dest.Email, src => src.ContactInfo.Email)
                .Map(dest => dest.PhoneNumber, src => src.ContactInfo.PhoneNumber)
                .Map(dest => dest.DateOfBirth, src => src.PersonalInfo.DateOfBirth)
                .Map(dest => dest.Gender, src => src.PersonalInfo.Gender)
                .Map(dest => dest.Age, src => CalculateAge(src.PersonalInfo.DateOfBirth))
                .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.MembershipNumber, src => src.MembershipNumber)
                .Map(dest => dest.PreferredLanguage, src => src.PreferredLanguage)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.HasActiveSubscription, src => src.Subscriptions.Any(s => s.IsActive));

            // UserDto → User
            config.NewConfig<UserDto, User>()
                .Map(dest => dest.PersonalInfo,
                    src => new PersonalInfo(src.FirstName, src.LastName, src.DateOfBirth, src.Gender))
                .Map(dest => dest.ContactInfo,
                    src => new ContactInfo(src.Email, src.PhoneNumber))
                .Map(dest => dest.ProfileImageUrl, src => src.ProfileImageUrl)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.MembershipNumber, src => src.MembershipNumber)
                .Map(dest => dest.PreferredLanguage, src => src.PreferredLanguage)
                // CreatedAt and UpdatedAt are usually set internally
                .Ignore(dest => dest.Subscriptions)
                .Ignore(dest => dest.WorkoutSessions);
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

