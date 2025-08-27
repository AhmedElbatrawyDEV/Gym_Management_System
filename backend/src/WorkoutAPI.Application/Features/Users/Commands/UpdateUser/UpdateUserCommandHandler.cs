using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutAPI.Application.Common.Exceptions;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Application.Features.Users.Commands.UpdateUser;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Application.Features.Users.Commands.UpdateUserPersonalInfo
{

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand,Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new NotFoundException(nameof(User), request.Id);

            // Check if email is unique (excluding current user)
            var isEmailUnique = await _userRepository.IsEmailUniqueAsync(request.Email, request.Id, cancellationToken);
            if (!isEmailUnique)
                throw new InvalidOperationException("Email is already in use by another user");

            var personalInfo = new PersonalInfo(request.FirstName, request.LastName, request.DateOfBirth, request.Gender);
            var contactInfo = new ContactInfo(request.Email, request.PhoneNumber);

            user.UpdatePersonalInfo(personalInfo);
            user.UpdateContactInfo(contactInfo);

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

}
