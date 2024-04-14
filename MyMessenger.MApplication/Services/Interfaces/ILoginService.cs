using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<TokensDTO> LoggingIn(LoginDTO user);
        public Task<TokensDTO> RefreshTokens(TokensDTO tokens);
    }
}
