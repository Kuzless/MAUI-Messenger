
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Application.DTO.AuthDTOs;

namespace MyMessenger.Application.Services.Interfaces
{
    public interface ITokenValidatorService
    {
        public SecurityToken? ValidateData(TokensDTO tokens);
    }
}
