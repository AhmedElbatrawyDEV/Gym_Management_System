using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CheckOut;

public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, Result>
{
    private readonly IAttendanceRecordRepository _attendanceRecordRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckOutCommandHandler(
        IAttendanceRecordRepository attendanceRecordRepository,
        IUnitOfWork unitOfWork)
    {
        _attendanceRecordRepository = attendanceRecordRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CheckOutCommand request, CancellationToken cancellationToken)
    {
        var attendanceRecord = await _attendanceRecordRepository.GetByIdAsync(request.AttendanceRecordId, cancellationToken) ?? throw new NotFoundException("AttendanceRecord", request.AttendanceRecordId);

        if (attendanceRecord.CheckOutTime.HasValue)
            throw new InvalidOperationException("User is already checked out");

        attendanceRecord.CheckOut();

        await _unitOfWork.BeginAsync();
        try
        {
            await _attendanceRecordRepository.UpdateAsync(attendanceRecord, cancellationToken);
            await _unitOfWork.Commit();
            return Result.Success();
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}