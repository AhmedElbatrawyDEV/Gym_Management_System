using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CreateWorkoutSession;

public class CreateWorkoutSessionCommandHandler : IRequestHandler<CreateWorkoutSessionCommand, Result<Guid>>
{
    private readonly IWorkoutSessionRepository _workoutSessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkoutSessionCommandHandler(
        IWorkoutSessionRepository workoutSessionRepository,
        IUserRepository userRepository,
        ITrainerRepository trainerRepository,
        IUnitOfWork unitOfWork)
    {
        _workoutSessionRepository = workoutSessionRepository;
        _userRepository = userRepository;
        _trainerRepository = trainerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var userExists = await _userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
            throw new NotFoundException("User", request.UserId);

        // Validate trainer exists if specified
        if (request.TrainerId.HasValue)
        {
            var trainerExists = await _trainerRepository.ExistsAsync(request.TrainerId.Value, cancellationToken);
            if (!trainerExists)
                throw new NotFoundException("Trainer", request.TrainerId.Value);
        }

        var session = WorkoutSession.CreateNew(request.UserId, request.Title, request.StartTime, request.TrainerId);

        await _unitOfWork.BeginAsync();
        try
        {
            await _workoutSessionRepository.AddAsync(session, cancellationToken);
            await _unitOfWork.Commit();
            return Result<Guid>.Success(session.Id);
        }
        catch
        {
            await _unitOfWork.Rollback();
            throw;
        }
    }
}
