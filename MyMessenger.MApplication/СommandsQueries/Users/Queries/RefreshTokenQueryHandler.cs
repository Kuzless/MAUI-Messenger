using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.Services.Interfaces;

namespace MyMessenger.MApplication.СommandsQueries.Users.Queries
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, TokensDTO>
    {
        private readonly ILoginService loginService;
        public RefreshTokenQueryHandler(ILoginService loginService)
        {
            this.loginService = loginService;
        }
        public async Task<TokensDTO> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            return await loginService.RefreshTokens(request.Tokens);
        }
    }
}
