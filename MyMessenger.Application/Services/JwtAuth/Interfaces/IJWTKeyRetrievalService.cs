using Microsoft.IdentityModel.Tokens;
namespace MyMessenger.Application.Services.JwtAuth.Interfaces
{
    public interface IJWTKeyRetrievalService
    {
        SymmetricSecurityKey GetJwtSecretKey();
    }
}
