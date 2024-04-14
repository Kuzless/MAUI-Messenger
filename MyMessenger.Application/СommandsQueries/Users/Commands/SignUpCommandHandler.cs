using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Application.Services.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Users.Commands
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, ResponseDTO>
    {
        private readonly ISignUpService signUpService;
        public SignUpCommandHandler(ISignUpService signUpService)
        {
             this.signUpService = signUpService;
        }
        public Task<ResponseDTO> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            return signUpService.SignUp(request.User);
        }
    }
}
