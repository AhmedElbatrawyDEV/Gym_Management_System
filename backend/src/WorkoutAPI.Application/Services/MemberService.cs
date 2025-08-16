using Mapster;
using Microsoft.Extensions.Logging;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Services;

public interface IMemberService
{
    Task<MemberResponse> CreateMemberAsync(CreateMemberRequest request);
    Task<MemberResponse> UpdateMemberAsync(Guid memberId, UpdateMemberRequest request);
    Task<MemberResponse?> GetMemberByIdAsync(Guid memberId);
    Task<MemberResponse?> GetMemberByUserIdAsync(Guid userId);
    Task<IEnumerable<MemberResponse>> GetActiveMembersAsync();
    Task<IEnumerable<MemberResponse>> GetMembersByMembershipTypeAsync(MembershipType membershipType);
    Task<IEnumerable<MemberResponse>> GetExpiringMembershipsAsync(int daysFromNow);
    Task<bool> DeleteMemberAsync(Guid memberId);
}

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MemberService> _logger;

    public MemberService(IUnitOfWork unitOfWork, ILogger<MemberService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<MemberResponse> CreateMemberAsync(CreateMemberRequest request)
    {
        _logger.LogInformation("Creating new member for user ID: {UserId}", request.UserId);

        // Check if user exists
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {request.UserId} not found");
        }

        // Check if member already exists for this user
        var existingMember = await _unitOfWork.Members.GetByUserIdAsync(request.UserId);
        if (existingMember != null)
        {
            throw new InvalidOperationException($"Member already exists for user ID {request.UserId}");
        }

        var member = request.Adapt<Member>();
        await _unitOfWork.Members.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Member created successfully with ID: {MemberId}", member.Id);

        var result = await GetMemberByIdAsync(member.Id);
        return result!;
    }

    public async Task<MemberResponse> UpdateMemberAsync(Guid memberId, UpdateMemberRequest request)
    {
        _logger.LogInformation("Updating member with ID: {MemberId}", memberId);

        var member = await _unitOfWork.Members.GetByIdAsync(memberId);
        if (member == null)
        {
            throw new ArgumentException($"Member with ID {memberId} not found");
        }

        member.MembershipStartDate = request.MembershipStartDate;
        member.MembershipEndDate = request.MembershipEndDate;
        member.MembershipType = request.MembershipType;
        member.IsActiveMember = request.IsActiveMember;
        member.SetUpdated();

        _unitOfWork.Members.Update(member);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Member updated successfully with ID: {MemberId}", memberId);

        var result = await GetMemberByIdAsync(memberId);
        return result!;
    }

    public async Task<MemberResponse?> GetMemberByIdAsync(Guid memberId)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(memberId);
        if (member == null) return null;

        return new MemberResponse(
            member.Id,
            member.UserId,
            member.User.FirstName,
            member.User.LastName,
            member.User.Email,
            member.MembershipStartDate,
            member.MembershipEndDate,
            member.MembershipType,
            member.IsActiveMember,
            member.CreatedAt
        );
    }

    public async Task<MemberResponse?> GetMemberByUserIdAsync(Guid userId)
    {
        var member = await _unitOfWork.Members.GetByUserIdAsync(userId);
        if (member == null) return null;

        return new MemberResponse(
            member.Id,
            member.UserId,
            member.User.FirstName,
            member.User.LastName,
            member.User.Email,
            member.MembershipStartDate,
            member.MembershipEndDate,
            member.MembershipType,
            member.IsActiveMember,
            member.CreatedAt
        );
    }

    public async Task<IEnumerable<MemberResponse>> GetActiveMembersAsync()
    {
        var members = await _unitOfWork.Members.GetActiveMembersAsync();
        return members.Select(m => new MemberResponse(
            m.Id,
            m.UserId,
            m.User.FirstName,
            m.User.LastName,
            m.User.Email,
            m.MembershipStartDate,
            m.MembershipEndDate,
            m.MembershipType,
            m.IsActiveMember,
            m.CreatedAt
        ));
    }

    public async Task<IEnumerable<MemberResponse>> GetMembersByMembershipTypeAsync(MembershipType membershipType)
    {
        var members = await _unitOfWork.Members.GetMembersByMembershipTypeAsync(membershipType);
        return members.Select(m => new MemberResponse(
            m.Id,
            m.UserId,
            m.User.FirstName,
            m.User.LastName,
            m.User.Email,
            m.MembershipStartDate,
            m.MembershipEndDate,
            m.MembershipType,
            m.IsActiveMember,
            m.CreatedAt
        ));
    }

    public async Task<IEnumerable<MemberResponse>> GetExpiringMembershipsAsync(int daysFromNow)
    {
        var expirationDate = DateTime.UtcNow.AddDays(daysFromNow);
        var members = await _unitOfWork.Members.GetExpiringMembershipsAsync(expirationDate);
        return members.Select(m => new MemberResponse(
            m.Id,
            m.UserId,
            m.User.FirstName,
            m.User.LastName,
            m.User.Email,
            m.MembershipStartDate,
            m.MembershipEndDate,
            m.MembershipType,
            m.IsActiveMember,
            m.CreatedAt
        ));
    }

    public async Task<bool> DeleteMemberAsync(Guid memberId)
    {
        _logger.LogInformation("Deleting member with ID: {MemberId}", memberId);

        var member = await _unitOfWork.Members.GetByIdAsync(memberId);
        if (member == null)
        {
            return false;
        }

        // Set as inactive instead of hard delete
        member.IsActiveMember = false;
        member.SetUpdated();

        _unitOfWork.Members.Update(member);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Member soft deleted successfully with ID: {MemberId}", memberId);

        return true;
    }
}

