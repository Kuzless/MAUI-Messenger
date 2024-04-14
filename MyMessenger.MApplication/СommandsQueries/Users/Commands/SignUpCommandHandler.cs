using MediatR;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.Services.Interfaces;

namespace MyMessenger.MApplication.СommandsQueries.Users.Commands
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
