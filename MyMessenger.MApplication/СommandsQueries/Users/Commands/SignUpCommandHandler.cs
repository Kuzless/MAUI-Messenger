using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.Services.Interfaces;

namespace MyMessenger.MApplication.СommandsQueries.Users.Commands
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResponseDTO>
    {
        private readonly ISignUpService signUpService;
        public SignUpCommandHandler(ISignUpService signUpService)
        {
             this.signUpService = signUpService;
        }
        public Task<SignUpResponseDTO> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            return signUpService.SignUp(request.User);
        }
    }
}
