using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.StartWorkoutSession;

public class StartWorkoutSessionCommandHandler : IRequestHandler<StartWorkoutSessionCommand, Result>
{
    private readonly IWorkoutSessionRepository _workoutSessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartWorkoutSessionCommandHandler(
        IWorkoutSessionRepository workoutSessionRepository,
        IUnitOfWork unitOfWork)
    {
        _workoutSessionRepository = workoutSessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(StartWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _workoutSessionRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("WorkoutSession", request.Id);

        session.StartSession();

        await _unitOfWork.BeginAsync();
        try
        {
            await _workoutSessionRepository.UpdateAsync(session, cancellationToken);
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
