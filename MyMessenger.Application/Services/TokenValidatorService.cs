using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MyMessenger.Application.Services
{
    public class TokenValidatorService : ITokenValidatorService
    {
        private readonly IConfiguration configuration;
        public TokenValidatorService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public SecurityToken? ValidateData(TokensDTO tokens)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"])),
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateAudience = false,
            };
            var principal = handler.ValidateToken(tokens.accessToken, tokenValidationParameters, out SecurityToken validatedToken);
            return validatedToken;
        }
    }
}
