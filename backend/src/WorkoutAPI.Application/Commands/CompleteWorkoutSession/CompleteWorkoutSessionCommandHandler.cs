using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Commands.CompleteWorkoutSession;

public class CompleteWorkoutSessionCommandHandler : IRequestHandler<CompleteWorkoutSessionCommand, Result>
{
    private readonly IWorkoutSessionRepository _workoutSessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteWorkoutSessionCommandHandler(
        IWorkoutSessionRepository workoutSessionRepository,
        IUnitOfWork unitOfWork)
    {
        _workoutSessionRepository = workoutSessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CompleteWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _workoutSessionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (session == null)
            throw new NotFoundException("WorkoutSession", request.Id);

        session.CompleteSession(request.Notes);

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