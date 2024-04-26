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

namespace MyMessenger.Application.Tests.Services
{
    public class LoginServiceTests
    {
        private Mock<IConfiguration> configuration;
        private Mock<UserManager<User>> userManager;
        private Mock<IJWTGeneratorService> jwtGenerator;
        private ILoginService sut;
        public LoginServiceTests()
        {
            configuration = new Mock<IConfiguration>();
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
            sut = new LoginService(userManager.Object, jwtGenerator.Object, configuration.Object);

        }
        [Fact]
        public async Task LoggingIn_ReturnsTokens_IfValidCredentials()
        {
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
            userManager.Verify(m => m.FindByEmailAsync(loginDTO.Email), Times.Once);
            userManager.Verify(m => m.CheckPasswordAsync(user, loginDTO.Password), Times.Once);
            jwtGenerator.Verify(m => m.GenerateToken(user.Email, user.Id, user.Name), Times.Once);
            userManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }
        [Fact]
        public async Task RefreshTokens_ReturnsNewTokens()
        {
            LoginDTO loginDTO;
            User user;
            TokensDTO tokensDTO;
            ProvideData(out loginDTO, out user, out tokensDTO);
            userManager.Setup(m => m.FindByIdAsync(user.Id)).ReturnsAsync(user);
            jwtGenerator.Setup(m => m.GenerateToken(user.Email, user.Id, user.Name)).Returns(tokensDTO);
            userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await sut.RefreshTokens(tokensDTO);

            Assert.Equal(tokensDTO, result);
            userManager.Verify(m => m.FindByIdAsync(user.Id), Times.Once);
            jwtGenerator.Verify(m => m.GenerateToken(user.Email, user.Id, user.Name), Times.Once);
            userManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }
        private void ProvideData(out LoginDTO loginDTO, out User user, out TokensDTO tokensDTO)
        {
            loginDTO = new LoginDTO { Email = "email@gmail.com", Password = "123123" };
            user = new User { Email = "email@gmail.com", Id = "userId", Name = "User" };
            tokensDTO = new TokensDTO { accessToken = "accessToken", refreshToken = "refreshToken" };
        }
    }
}
