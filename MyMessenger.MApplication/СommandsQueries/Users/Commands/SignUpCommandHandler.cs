using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
