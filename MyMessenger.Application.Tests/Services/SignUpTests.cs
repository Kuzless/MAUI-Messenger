
using Microsoft.AspNetCore.Identity;
using Moq;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Application.Services;
using MyMessenger.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyMessenger.Application.Tests.Services
{
    public class SignUpTests
    {
        private Mock<UserManager<User>> userManager;
        private ISignUpService signUpService;

        public SignUpTests()
        {
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
            signUpService = new SignUpService(userManager.Object);
        }
        [Fact]
        public async Task SignUp_InvokesCreateAsync()
        {
            var signUpDto = new SignUpDTO { Email = "test@gmail.com", Name = "User1", UserName = "user1", Password = "123" };
            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), signUpDto.Password)).ReturnsAsync(IdentityResult.Success);

            var result = await signUpService.SignUp(signUpDto);

            userManager.Verify(m => m.CreateAsync(It.IsAny<User>(), signUpDto.Password), Times.Once);
        }
        [Fact]
        public async Task SignUp_ReturnsData()
        {
            var signUpDto = new SignUpDTO { Email = "test@gmail.com", Name = "User1", UserName = "user1", Password = "123" };
            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), signUpDto.Password)).ReturnsAsync(IdentityResult.Success);

            var result = await signUpService.SignUp(signUpDto);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task SignUp_ReturnsSuccessResponse_WhenUserCreatedSuccessfully()
        {
            var signUpDto = new SignUpDTO { Email = "test@gmail.com", Name = "User1", UserName = "user1", Password = "123" };
            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), signUpDto.Password)).ReturnsAsync(IdentityResult.Success);

            var result = await signUpService.SignUp(signUpDto);

            Assert.NotNull(result);
            Assert.True(result.isSuccessful);
            Assert.Equal("User signed up successfully.", result.message);
        }

        [Fact]
        public async Task SignUp_ReturnsErrorResponse_WhenUserCreationFails()
        {
            var signUpDto = new SignUpDTO { Email = "test@gmail.com", Name = "User1", UserName = "user1", Password = "123" };
            var errorResult = IdentityResult.Failed(new IdentityError { Description = "Duplicate email." });
            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), signUpDto.Password)).ReturnsAsync(errorResult);

            var result = await signUpService.SignUp(signUpDto);

            Assert.NotNull(result);
            Assert.False(result.isSuccessful);
            Assert.Equal("Failed to sign up.", result.message);
        }

    }
}
