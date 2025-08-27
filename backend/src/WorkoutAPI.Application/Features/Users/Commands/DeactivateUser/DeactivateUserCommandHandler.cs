using MediatR;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Interfaces;

namespace WorkoutAPI.Application.Features.Users.Commands.DeactivateUser;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException(nameof(User), request.Id);

        user.Deactivate();

        await _unitOfWork.BeginAsync();
        try
        {
            await _userRepository.UpdateAsync(user, cancellationToken);
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


