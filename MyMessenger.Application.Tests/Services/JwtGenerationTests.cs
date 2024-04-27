using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using MyMessenger.Application.Services.JwtAuth;

namespace MyMessenger.Application.Tests.Services
{
    public class JwtGenerationTests
    {
        private Mock<IConfiguration> configuration;
        private JWTGeneratorService sut;
        public JwtGenerationTests()
        {
            configuration = new Mock<IConfiguration>();
            sut = new JWTGeneratorService(configuration.Object);
        }
        [Fact]
        public void GenerateToken_ReturnsValidTokens()
        {
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("SecretTestKey_1231235434123438_483750934392_4849732393438");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("issuer");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("audience");

            var tokens = sut.GenerateToken("newemail@gmail.com", "UserId", "User");

            Assert.NotNull(tokens.accessToken);
            Assert.NotNull(tokens.refreshToken);
        }

        [Fact]
        public void GenerateToken_ReturnsValidRefreshToken()
        {
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("SecretTestKey_1231235434123438_483750934392_4849732393438");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("issuer");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("audience");

            var tokens = sut.GenerateToken("newemail@gmail.com", "UserId", "User");

            Assert.NotNull(tokens.refreshToken);
            Assert.Equal(32, tokens.refreshToken.Length);
        }
    }
}
