using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace MyMessenger.Application.Services
{
    public class TokenValidatorService : ITokenValidatorService
    {
        private readonly IConfiguration configuration;
        private readonly IJWTKeyRetrievalService keyRetrievalService;
        public TokenValidatorService(IConfiguration configuration, IJWTKeyRetrievalService keyRetrievalService)
        {
            this.configuration = configuration;
            this.keyRetrievalService = keyRetrievalService;
        }
        public SecurityToken? ValidateData(TokensDTO tokens)
        {
            var handler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey? securityKey = keyRetrievalService.GetJwtSecretKey();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateAudience = false,
            };
            var principal = handler.ValidateToken(tokens.accessToken, tokenValidationParameters, out SecurityToken validatedToken);
            return validatedToken;
        }
    }
}
