using Microsoft.AspNetCore.Identity;
using Moq;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using MyMessenger.Application.Services;
using MyMessenger.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MyMessenger.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MyMessenger.Application.Tests.Services
{
    public class LoginTests
    {
        private Mock<UserManager<User>> userManager;
        private Mock<IJWTGeneratorService> jwtGenerator;
        private Mock<ITokenValidatorService> tokenValidatorService;
        public LoginTests()
        {
            tokenValidatorService = new Mock<ITokenValidatorService>();
            userManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);
            jwtGenerator = new Mock<IJWTGeneratorService>();

        }
        [Fact]
        public async Task LoggingIn_InvokesLoggingIn()
        {
            var sut = new LoginService(userManager.Object, jwtGenerator.Object, tokenValidatorService.Object);
            LoginDTO loginDTO;
            User user;
            TokensDTO tokensDTO;
            ProvideData(out loginDTO, out user, out tokensDTO);
            userManager.Setup(m => m.FindByEmailAsync(loginDTO.Email)).ReturnsAsync(user);
            userManager.Setup(m => m.CheckPasswordAsync(user, loginDTO.Password)).ReturnsAsync(true);
            jwtGenerator.Setup(m => m.GenerateToken(user.Email, user.Id, user.Name)).Returns(tokensDTO);
            userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await sut.LoggingIn(loginDTO);

            userManager.Verify(m => m.FindByEmailAsync(loginDTO.Email), Times.Once);
            userManager.Verify(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            jwtGenerator.Verify(m => m.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            userManager.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public async Task LoggingIn_ReturnsData()
        {
            var sut = new LoginService(userManager.Object, jwtGenerator.Object, tokenValidatorService.Object);
            LoginDTO loginDTO;
            User user;
            TokensDTO tokensDTO;
            ProvideData(out loginDTO, out user, out tokensDTO);
            userManager.Setup(m => m.FindByEmailAsync(loginDTO.Email)).ReturnsAsync(user);
            userManager.Setup(m => m.CheckPasswordAsync(user, loginDTO.Password)).ReturnsAsync(true);
            jwtGenerator.Setup(m => m.GenerateToken(user.Email, user.Id, user.Name)).Returns(tokensDTO);
            userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await sut.LoggingIn(loginDTO);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task LoggingIn_ReturnsTokens_IfValidCredentials()
        {
            var sut = new LoginService(userManager.Object, jwtGenerator.Object, tokenValidatorService.Object);
            LoginDTO loginDTO;
            User user;
            TokensDTO tokensDTO;
            ProvideData(out loginDTO, out user, out tokensDTO);
            userManager.Setup(m => m.FindByEmailAsync(loginDTO.Email)).ReturnsAsync(user);
            userManager.Setup(m => m.CheckPasswordAsync(user, loginDTO.Password)).ReturnsAsync(true);
            jwtGenerator.Setup(m => m.GenerateToken(user.Email, user.Id, user.Name)).Returns(tokensDTO);
            userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await sut.LoggingIn(loginDTO);

            Assert.Equal(tokensDTO, result);
        }
        [Fact]
        public async Task LoggingIn_DoesNotReturnTokens_IfInvalidCredentials()
        {
            var sut = new LoginService(userManager.Object, jwtGenerator.Object, tokenValidatorService.Object);
            var invalidLoginDTO = new LoginDTO { Email = "invalid@gmail.com", Password = "invalidpassword" };
            userManager.Setup(m => m.FindByEmailAsync(invalidLoginDTO.Email)).ReturnsAsync((User)null);

            var result = await sut.LoggingIn(invalidLoginDTO);

            Assert.Null(result.accessToken);
            Assert.Null(result.refreshToken);
            userManager.Verify(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            jwtGenerator.Verify(m => m.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            userManager.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);

        }
        private void ProvideData(out LoginDTO loginDTO, out User user, out TokensDTO tokensDTO)
        {
            loginDTO = new LoginDTO { Email = "email@gmail.com", Password = "123123" };
            user = new User { Email = "email@gmail.com", Id = "userId", Name = "User" };
            tokensDTO = new TokensDTO { accessToken = "accessToken", refreshToken = "refreshToken" };
        }
    }
}
