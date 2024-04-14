using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.Services.JwtAuth.Interfaces
{
    public interface IJWTGeneratorService
    {
        public TokensDTO GenerateToken(string email, string id);
    }
}
