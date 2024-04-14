using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Domain.Entities;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.DTO.UserDTOs;
using MyMessenger.MApplication.Services.JwtAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.Services.JwtAuth
{
    public class JWTGeneratorService : IJWTGeneratorService
    {
        private readonly JWTOptions options;
        public JWTGeneratorService(IOptions<JWTOptions> options)
        {
            this.options = options.Value;
        }
        public TokensDTO GenerateToken(string email, string id)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(options.SecretKey)),
                SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                options.Issuer,
                options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
                );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            string refreshTokenValue = GenerateRefreshToken();
            return new TokensDTO { accessToken = tokenValue, refreshToken = refreshTokenValue };
        }
        private string GenerateRefreshToken()
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rng = new RNGCryptoServiceProvider();
            var randomBytes = new byte[32];
            rng.GetBytes(randomBytes);

            var chars = new char[32];
            var numAllowedChars = allowedChars.Length;

            for (int i = 0; i < 32; i++)
            {
                chars[i] = allowedChars[randomBytes[i] % numAllowedChars];
            }

            return new string(chars);
        }
    }
}
