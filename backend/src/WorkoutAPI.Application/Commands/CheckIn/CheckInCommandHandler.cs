using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CheckIn;

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, Result<Guid>>
{
    private readonly IAttendanceRecordRepository _attendanceRecordRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckInCommandHandler(
        IAttendanceRecordRepository attendanceRecordRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _attendanceRecordRepository = attendanceRecordRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        // Check if user is already checked in
        var currentCheckIn = await _attendanceRecordRepository.GetCurrentCheckInAsync(request.UserId, cancellationToken) ?? throw new InvalidOperationException("User is already checked in");

        var attendanceRecord = AttendanceRecord.CreateNew(request.UserId, request.ActivityType);

        await _unitOfWork.BeginAsync();
        try
        {
            await _attendanceRecordRepository.AddAsync(attendanceRecord, cancellationToken);
            await _unitOfWork.Commit();
            return Result<Guid>.Success(attendanceRecord.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}