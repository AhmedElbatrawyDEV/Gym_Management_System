using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Queries.GetAttendanceRecords;
public class GetAttendanceRecordsQueryHandler : IRequestHandler<GetAttendanceRecordsQuery, PaginatedResult<AttendanceRecordDto>>
{
    private readonly IAttendanceRecordRepository _attendanceRecordRepository;
    private readonly IUserRepository _userRepository;

    public GetAttendanceRecordsQueryHandler(
        IAttendanceRecordRepository attendanceRecordRepository,
        IUserRepository userRepository)
    {
        _attendanceRecordRepository = attendanceRecordRepository;
        _userRepository = userRepository;
    }

    public async Task<PaginatedResult<AttendanceRecordDto>> Handle(GetAttendanceRecordsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            return PaginatedResult<AttendanceRecordDto>.Failure($"User with ID {request.UserId} not found.");

        var userName = user.PersonalInfo.FullName;

        var (records, totalCount) = await _attendanceRecordRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            request.UserId,
            request.FromDate,
            request.ToDate,
            cancellationToken);

        var recordDtos = records.Select(record => new AttendanceRecordDto
        {
            Id = record.Id,
            UserId = record.UserId,
            UserName = userName,
            CheckInTime = record.CheckInTime,
            CheckOutTime = record.CheckOutTime,
            DurationMinutes = record.DurationMinutes,
            ActivityType = record.ActivityType,
            IsCheckedIn = record.IsCheckedIn
        }).ToList().AsReadOnly();

        return PaginatedResult<AttendanceRecordDto>.Success(recordDtos, request.PageNumber, request.PageSize, totalCount);
    }
}

