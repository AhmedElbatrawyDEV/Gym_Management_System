using MediatR;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Interfaces;

public class GetClassBookingsQueryHandler : IRequestHandler<GetClassBookingsQuery, Result<List<ClassBookingDto>>>
{
    private readonly IClassBookingRepository _classBookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IClassScheduleRepository _classScheduleRepository;

    public GetClassBookingsQueryHandler(
        IClassBookingRepository classBookingRepository,
        IUserRepository userRepository,
        IClassScheduleRepository classScheduleRepository)
    {
        _classBookingRepository = classBookingRepository;
        _userRepository = userRepository;
        _classScheduleRepository = classScheduleRepository;
    }

    public async Task<Result<List<ClassBookingDto>>> Handle(GetClassBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _classBookingRepository.GetFilteredAsync(
            request.UserId,
            request.ClassScheduleId,
            request.Status,
            request.FromDate,
            request.ToDate,
            cancellationToken);

        var userIds = bookings.Select(b => b.UserId).Distinct().ToList();
        var scheduleIds = bookings.Select(b => b.ClassScheduleId).Distinct().ToList();

        var users = await _userRepository.GetByIdsAsync(userIds, cancellationToken);
        var schedules = await _classScheduleRepository.GetByIdsAsync(scheduleIds, cancellationToken);

        var userLookup = users.ToDictionary(u => u.Id, u => u.PersonalInfo.FullName);
        var scheduleLookup = schedules.ToDictionary(s => s.Id, s => new { s.GymClass.Name, s.StartTime, s.EndTime });

        var result = bookings.Select(booking =>
        {
            var schedule = scheduleLookup.GetValueOrDefault(booking.ClassScheduleId);
            return new ClassBookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                UserName = userLookup.GetValueOrDefault(booking.UserId, string.Empty),
                ClassScheduleId = booking.ClassScheduleId,
                ClassName = schedule?.Name ?? string.Empty,
                ClassStartTime = schedule?.StartTime ?? DateTime.MinValue,
                ClassEndTime = schedule?.EndTime ?? DateTime.MinValue,
                BookingDate = booking.BookingDate,
                Status = booking.Status
            };
        }).ToList();

        return Result<List<ClassBookingDto>>.Success(result);
    }
}