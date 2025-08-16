using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Application.DTOs;

public record CreateMemberRequest(
    Guid UserId,
    DateTime MembershipStartDate,
    DateTime MembershipEndDate,
    MembershipType MembershipType
);

public record UpdateMemberRequest(
    DateTime MembershipStartDate,
    DateTime MembershipEndDate,
    MembershipType MembershipType,
    bool IsActiveMember
);

public record MemberResponse(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    DateTime MembershipStartDate,
    DateTime MembershipEndDate,
    MembershipType MembershipType,
    bool IsActiveMember,
    DateTime CreatedAt
);


