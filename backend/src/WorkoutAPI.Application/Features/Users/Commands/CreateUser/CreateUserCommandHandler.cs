using MediatR;
using WorkoutAPI.Application.Common.Interfaces;
using WorkoutAPI.Application.Common.Models;
using WorkoutAPI.Domain.Aggregates;
using WorkoutAPI.Domain.Interfaces;
using WorkoutAPI.Domain.ValueObjects;

namespace WorkoutAPI.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Check if email is unique
            var isEmailUnique = await _userRepository.IsEmailUniqueAsync(request.Email, cancellationToken: cancellationToken);
            if (!isEmailUnique)
                throw new InvalidOperationException("User with this email already exists");

            var personalInfo = new PersonalInfo(request.FirstName, request.LastName, request.DateOfBirth, request.Gender);
            var contactInfo = new ContactInfo(request.Email, request.PhoneNumber);

            var user = User.CreateNew(personalInfo, contactInfo, request.PreferredLanguage);

            await _unitOfWork.BeginAsync();
            try
            {
                await _userRepository.AddAsync(user, cancellationToken);
                await _unitOfWork.Commit();

                // Send welcome email
                await _emailService.SendWelcomeEmailAsync(request.Email, request.FirstName);

                return user.Id;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
