using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Queries.GetAttendanceRecords;
public class GetAttendanceRecordsQuery : BasePaginatedQuery<AttendanceRecordDto>
{
    public Guid UserId { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}