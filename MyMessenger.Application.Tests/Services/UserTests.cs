
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MyMessenger.Application.Services;
using MyMessenger.Domain.Entities;

namespace MyMessenger.Application.Tests.Services
{
    public class UserTests
    {
        private readonly Mock<UserManager<User>> userManager;

        public UserTests()
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
        }
        [Fact]
        public async Task GetUserById_InvokesFindByIdAsync()
        {
            var sut = new UserService(userManager.Object);
            var userId = "UserId";
            var expectedUser = new User { Id = userId, UserName = "testUser" };
            userManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(expectedUser);

            var result = await sut.GetUserById(userId);

            userManager.Verify(m => m.FindByIdAsync(userId), Times.Once);
        }
        [Fact]
        public async Task GetUserById_ReturnsData()
        {
            var sut = new UserService(userManager.Object);
            var userId = "UserId";
            var expectedUser = new User { Id = userId, UserName = "testUser" };
            userManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(expectedUser);

            var result = await sut.GetUserById(userId);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetUserById_ReturnsCorrectUser()
        {
            var sut = new UserService(userManager.Object);
            var userId = "UserId";
            var expectedUser = new User { Id = userId, UserName = "testUser" };
            userManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(expectedUser);

            var result = await sut.GetUserById(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
        }
        [Fact]
        public async Task GetUserByUserName_InvokesFindByNameAsync()
        {
            var sut = new UserService(userManager.Object);
            var userName = "testUser";
            var expectedUser = new User { Id = "User1", UserName = userName };
            userManager.Setup(m => m.FindByNameAsync(userName)).ReturnsAsync(expectedUser);

            var result = await sut.GetUserByUserName(userName);

            userManager.Verify(m => m.FindByNameAsync(userName), Times.Once);
        }
        [Fact]
        public async Task GetUserByUserName_ReturnsData()
        {
            var sut = new UserService(userManager.Object);
            var userName = "testUser";
            var expectedUser = new User { Id = "User1", UserName = userName };
            userManager.Setup(m => m.FindByNameAsync(userName)).ReturnsAsync(expectedUser);

            var result = await sut.GetUserByUserName(userName);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetUserByUserName_ReturnsCorrectUser()
        {
            var sut = new UserService(userManager.Object);
            var userName = "testUser";
            var expectedUser = new User { Id = "User1", UserName = userName };
            userManager.Setup(m => m.FindByNameAsync(userName)).ReturnsAsync(expectedUser);

            var result = await sut.GetUserByUserName(userName);

            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
        }
    }
}
