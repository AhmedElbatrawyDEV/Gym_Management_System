using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CreateClassBooking;

public class CreateClassBookingCommandHandler : IRequestHandler<CreateClassBookingCommand, Result<Guid>>
{
    private readonly IClassBookingRepository _classBookingRepository;
    private readonly IClassScheduleRepository _classScheduleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClassBookingCommandHandler(
        IClassBookingRepository classBookingRepository,
        IClassScheduleRepository classScheduleRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _classBookingRepository = classBookingRepository;
        _classScheduleRepository = classScheduleRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateClassBookingCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var userExists = await _userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
            throw new NotFoundException("User", request.UserId);

        // Validate class schedule exists and has available spots
        var classSchedule = await _classScheduleRepository.GetByIdAsync(request.ClassScheduleId, cancellationToken) ?? throw new NotFoundException("ClassSchedule", request.ClassScheduleId);

        if (!classSchedule.HasAvailableSpots)
            throw new InvalidOperationException("Class is at full capacity");

        var booking = ClassBooking.CreateNew(request.UserId, request.ClassScheduleId);

        await _unitOfWork.BeginAsync();
        try
        {
            // Enroll member in the class
            classSchedule.EnrollMember();

            await _classBookingRepository.AddAsync(booking, cancellationToken);
            await _classScheduleRepository.UpdateAsync(classSchedule, cancellationToken);

            await _unitOfWork.Commit();
            return Result<Guid>.Success(booking.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}
