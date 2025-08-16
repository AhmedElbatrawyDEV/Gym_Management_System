using Microsoft.EntityFrameworkCore;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Infrastructure.Data;

namespace WorkoutAPI.Infrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(WorkoutDbContext context) : base(context)
    {
    }

    public async Task<Member?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task<IEnumerable<Member>> GetActiveMembersAsync()
    {
        return await _dbSet
            .Include(m => m.User)
            .Where(m => m.IsActiveMember && m.User.IsActive)
            .OrderBy(m => m.User.FirstName)
            .ThenBy(m => m.User.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Member>> GetMembersByMembershipTypeAsync(MembershipType membershipType)
    {
        return await _dbSet
            .Include(m => m.User)
            .Where(m => m.MembershipType == membershipType && m.IsActiveMember)
            .OrderBy(m => m.User.FirstName)
            .ThenBy(m => m.User.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Member>> GetExpiringMembershipsAsync(DateTime expirationDate)
    {
        return await _dbSet
            .Include(m => m.User)
            .Where(m => m.MembershipEndDate <= expirationDate && m.IsActiveMember)
            .OrderBy(m => m.MembershipEndDate)
            .ToListAsync();
    }

    public async Task<Member?> GetMemberWithWorkoutPlansAsync(Guid memberId)
    {
        return await _dbSet
            .Include(m => m.User)
            .Include(m => m.EnrolledWorkoutPlans)
                .ThenInclude(uwp => uwp.WorkoutPlan)
                    .ThenInclude(wp => wp.Translations)
            .FirstOrDefaultAsync(m => m.Id == memberId);
    }
}

