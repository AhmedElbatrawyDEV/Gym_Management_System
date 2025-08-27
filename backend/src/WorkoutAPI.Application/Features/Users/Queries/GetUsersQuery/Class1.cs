//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WorkoutAPI.Application.Features.Users.Queries.GetUsersQuery;


//public record GetUsersQuery : IRequest<List<UserDto>>
//{
//    public string? SearchTerm { get; init; }
//    public UserStatus? Status { get; init; }
//}



//public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
//{
//    private readonly IUserRepository _userRepository;

//    public GetUsersQueryHandler(IUserRepository userRepository)
//    {
//        _userRepository = userRepository;
//    }

//    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
//    {
//        IEnumerable<Domain.Aggregates.User> users;

//        if (!string.IsNullOrEmpty(request.SearchTerm))
//        {
//            users = await _userRepository.SearchAsync(request.SearchTerm, cancellationToken);
//        }
//        else if (request.Status.HasValue)
//        {
//            users = await _userRepository.GetUsersByStatusAsync(request.Status.Value, cancellationToken);
//        }
//        else
//        {
//            users = await _userRepository.GetAllAsync(cancellationToken);
//        }

//        return users.Select(user => new UserDto
//        {
//            Id = user.Id,
//            FirstName = user.PersonalInfo.FirstName,
//            LastName = user.PersonalInfo.LastName,
//            FullName = user.PersonalInfo.FullName,
//            Email = user.ContactInfo.Email,
//            PhoneNumber = user.ContactInfo.PhoneNumber,
//            DateOfBirth = user.PersonalInfo.DateOfBirth,
//            Gender = user.PersonalInfo.Gender,
//            Age = user.PersonalInfo.Age,
//            ProfileImageUrl = user.ProfileImageUrl,
//            Status = user.Status,
//            MembershipNumber = user.MembershipNumber,
//            PreferredLanguage = user.PreferredLanguage,
//            CreatedAt = user.CreatedAt,
//            UpdatedAt = user.UpdatedAt,
//            HasActiveSubscription = user.HasActiveSubscription()
//        }).ToList();
//    }
//}

