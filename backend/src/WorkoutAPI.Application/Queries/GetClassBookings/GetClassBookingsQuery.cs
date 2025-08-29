using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Enums;

public class GetClassBookingsQuery : BaseQuery<List<ClassBookingDto>>
{
    public Guid? UserId { get; init; }
    public Guid? ClassScheduleId { get; init; }
    public BookingStatus? Status { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}
