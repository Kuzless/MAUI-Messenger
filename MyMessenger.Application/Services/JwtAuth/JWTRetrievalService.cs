using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MyMessenger.Application.Services.JwtAuth
{
    public class JWTRetrievalService : IJWTRetrievalService
    {
        private readonly IConfiguration configuration;
        public JWTRetrievalService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string? GetEmailByToken(TokensDTO tokens)
        {
            var token = ValidateData(tokens);
            var userEmailClaim = ((JwtSecurityToken)token).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            if (userEmailClaim != null)
            {
                return userEmailClaim.Value;
            }
            return null;
        }

        public string? GetIdByToken(TokensDTO tokens)
        {
            var token = ValidateData(tokens);
            var userIdClaim = ((JwtSecurityToken)token).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }
            return null;
        }
        private SecurityToken? ValidateData(TokensDTO tokens)
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
