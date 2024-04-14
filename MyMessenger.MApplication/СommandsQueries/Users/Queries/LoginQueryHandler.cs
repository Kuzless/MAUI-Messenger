using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.Services.Interfaces;

namespace MyMessenger.MApplication.СommandsQueries.Users.Queries
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
