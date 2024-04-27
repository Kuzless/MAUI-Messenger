using Microsoft.AspNetCore.Identity;
using MyMessenger.Domain.Entities;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace MyMessenger.Application.Services
{
    public class LoginService : ILoginService
    {
        private UserManager<User> userManager;
        private readonly IJWTGeneratorService jwtGenService;
        private readonly ITokenValidatorService tokenValidatorService;
        public LoginService(UserManager<User> userManager, IJWTGeneratorService jwtGenService, ITokenValidatorService tokenValidatorService)
        {
            this.userManager = userManager;
            this.jwtGenService = jwtGenService;
            this.tokenValidatorService = tokenValidatorService;
        }
        public async Task<TokensDTO> LoggingIn(LoginDTO user)
        {
            var validUser = await LoginCheck(user.Email, user.Password);
            if (validUser != null)
            {
                var tokens = jwtGenService.GenerateToken(validUser.Email, validUser.Id, validUser.Name);
                await AddTokenToDb(tokens, validUser);
                return tokens;
            }
            return new TokensDTO();
        }
        public async Task<TokensDTO> RefreshTokens(TokensDTO tokens)
        {
            var token = tokenValidatorService.ValidateData(tokens);
            var userId = ((JwtSecurityToken)token).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            if (userId != null)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (VerifyTokenFromDb(tokens, user))
                {
                    var newTokens = jwtGenService.GenerateToken(user.Email, user.Id, user.Name);
                    await AddTokenToDb(newTokens, user);
                    return newTokens;
                }
            }
            return new TokensDTO();
        }
        private async Task<User?> LoginCheck(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            return null;
        }
        private async Task AddTokenToDb(TokensDTO tokens, User user)
        {
            user.RefreshToken = tokens.refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            await userManager.UpdateAsync(user);
        }
        private bool VerifyTokenFromDb(TokensDTO tokens, User user)
        {
            if (user == null || user.RefreshToken != tokens.refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return false;
            }
            return true;
        }
    }
}
