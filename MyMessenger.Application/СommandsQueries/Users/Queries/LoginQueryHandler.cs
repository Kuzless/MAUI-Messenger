using MediatR;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Users.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, TokensDTO>
    {
        private readonly ILoginService loginService;
        public LoginQueryHandler(ILoginService loginService)
        {
            this.loginService = loginService;
        }
        public async Task<TokensDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return await loginService.LoggingIn(request.User);
        }
    }
}
