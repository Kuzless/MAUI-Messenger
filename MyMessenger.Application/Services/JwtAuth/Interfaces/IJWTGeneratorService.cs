using MyMessenger.Application.DTO.AuthDTOs;

namespace MyMessenger.Application.Services.JwtAuth.Interfaces
{
    public interface IJWTGeneratorService
    {
        public TokensDTO GenerateToken(string email, string id, string name);
    }
}
