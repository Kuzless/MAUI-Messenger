using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyMessenger.Application.Services.JwtAuth
{
    public class JWTGeneratorService : IJWTGeneratorService
    {
        private readonly IConfiguration configuration;
        public JWTGeneratorService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public TokensDTO GenerateToken(string email, string id, string name)
        {
            var claims = new Claim[]
            {
                new Claim("Name", name),
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
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
