using WorkoutAPI.Application.Common.Models;

namespace WorkoutAPI.Application.Commands.CheckOut;

public class CheckOutCommand : BaseCommand
{
    public Guid AttendanceRecordId { get; init; }
}
