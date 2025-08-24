using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Domain.Interfaces;

public interface IMemberRepository : IRepository<Member> {
    Task<Member?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Member>> GetActiveMembersAsync();
    Task<IEnumerable<Member>> GetMembersByMembershipTypeAsync(MembershipType membershipType);
    Task<IEnumerable<Member>> GetExpiringMembershipsAsync(DateTime expirationDate);
    Task<Member?> GetMemberWithWorkoutPlansAsync(Guid memberId);
}

