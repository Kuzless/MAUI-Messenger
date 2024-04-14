using MyMessenger.Application.DTO.AuthDTOs;

namespace MyMessenger.Application.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<TokensDTO> LoggingIn(LoginDTO user);
        public Task<TokensDTO> RefreshTokens(TokensDTO tokens);
    }
}
