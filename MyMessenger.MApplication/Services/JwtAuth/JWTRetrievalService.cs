using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Domain.Entities;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.Services.JwtAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.Services.JwtAuth
{
    public class JWTRetrievalService : IJWTRetrievalService
    {
        private readonly JWTOptions options;
        public JWTRetrievalService(IOptions<JWTOptions> options)
        {
            this.options = options.Value;
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SecretKey)),
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateAudience = false,
            };
            var principal = handler.ValidateToken(tokens.accessToken, tokenValidationParameters, out SecurityToken validatedToken);
            return validatedToken;
        }
    }
}
