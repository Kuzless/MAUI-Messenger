using AutoMapper;
using Moq;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.DTO.UserDTOs;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Application.СommandsQueries.Users.Queries;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.Tests.Queries
{
    public class UserQueriesTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mapperMock;
        public UserQueriesTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();
        }
        [Fact]
        public async Task GetAllUsersQueryHandler_ReturnsData()
        {

            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllUsersQuery(sort, 1, 10, "");
            var users = new List<User>
            {
                new User { Id = "User1", Name = "User1", Email = "user1@example.com" },
                new User { Id = "User1", Name = "User2", Email = "user2@example.com" },
                new User { Id = "User1", Name = "User3", Email = "user3@example.com" },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<User>, int> { { users, users.Count() } };
            unitOfWorkMock.Setup(uow => uow.GetRepository<User>().GetAll()).Returns(users);
            unitOfWorkMock.Setup(uow => uow.GetRepository<User>().FilterByQuery(users, sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserDTO>>(It.IsAny<IEnumerable<User>>())).Returns(It.IsAny<IEnumerable<UserDTO>>);
            var sut = new GetAllUsersQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllUsersQueryHandler_ReturnsCorrectNumberOfPages()
        {

            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllUsersQuery(sort, 1, 10, "");
            var users = new List<User>
            {
                new User { Id = "User1", Name = "User1", Email = "user1@example.com" },
                new User { Id = "User1", Name = "User2", Email = "user2@example.com" },
                new User { Id = "User1", Name = "User3", Email = "user3@example.com" },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<User>, int> { { users, users.Count() } };
            unitOfWorkMock.Setup(uow => uow.GetRepository<User>().GetAll()).Returns(users);
            unitOfWorkMock.Setup(uow => uow.GetRepository<User>().FilterByQuery(users, sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            var mappedResults = users.Select(user => new UserDTO { Name = user.Name, Email = user.Email }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserDTO>>(It.IsAny<IEnumerable<User>>())).Returns(mappedResults);
            var sut = new GetAllUsersQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.NumberOfPages);
        }
        [Fact]
        public async Task GetAllUsersQueryHandler_ReturnsCorrectMappedValues()
        {

            var sort = new Dictionary<string, bool> { { "field", true } };
            var query = new GetAllUsersQuery(sort, 1, 10, "");
            var users = new List<User>
            {
                new User { Id = "User1", Name = "User1", Email = "user1@example.com" },
                new User { Id = "User1", Name = "User2", Email = "user2@example.com" },
                new User { Id = "User1", Name = "User3", Email = "user3@example.com" },
            }.AsQueryable();
            var queryResult = new Dictionary<IEnumerable<User>, int> { { users, users.Count() } };
            unitOfWorkMock.Setup(uow => uow.GetRepository<User>().GetAll()).Returns(users);
            unitOfWorkMock.Setup(uow => uow.GetRepository<User>().FilterByQuery(users, sort, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(queryResult));
            var mappedResults = users.Select(user => new UserDTO { Name = user.Name, Email = user.Email }).AsEnumerable();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserDTO>>(It.IsAny<IEnumerable<User>>())).Returns(mappedResults);
            var sut = new GetAllUsersQueryHandler(unitOfWorkMock.Object, mapperMock.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(users.Count(), result.Data.Count());
        }
        [Fact]
        public async Task GetUserByIdQueryHandler_ReturnsData()
        {
            var userId = "User1";
            var userService = new Mock<IUserService>();
            var user = new User { Id = userId, Name = "User1", Email = "user1@gmail.com" };
            userService.Setup(service => service.GetUserById(userId)).Returns(Task.FromResult(user));
            var query = new GetUserByIdQuery(userId);
            var sut = new GetUserByIdQueryHandler(userService.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetUserByIdQueryHandler_ReturnsCorrectUser()
        {
            var userId = "User1";
            var userService = new Mock<IUserService>();
            var user = new User { Id = userId, Name = "User1", Email = "user1@gmail.com" };
            userService.Setup(service => service.GetUserById(userId)).Returns(Task.FromResult(user));
            var query = new GetUserByIdQuery(userId);
            var sut = new GetUserByIdQueryHandler(userService.Object);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }
        [Fact]
        public async Task LoginQueryHandler_ReturnsData()
        {
            var user = new LoginDTO { Email = "email@gmail.com", Password = "123123" };
            var tokensDTO = new TokensDTO { accessToken = "accessToken", refreshToken = "refreshToken" };
            var loginService = new Mock<ILoginService>();
            loginService.Setup(service => service.LoggingIn(user)).Returns(Task.FromResult(tokensDTO));
            var query = new LoginQuery(user);
            var sut = new LoginQueryHandler(loginService.Object);

            var result = sut.Handle(query, default);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task LoginQueryHandler_ReturnsCorrectTokensDTO()
        {
            var user = new LoginDTO { Email = "email@gmail.com", Password = "123123" };
            var tokensDTO = new TokensDTO { accessToken = "accessToken", refreshToken = "refreshToken" };
            var loginService = new Mock<ILoginService>();
            loginService.Setup(service => service.LoggingIn(user)).Returns(Task.FromResult(tokensDTO));
            var query = new LoginQuery(user);
            var sut = new LoginQueryHandler(loginService.Object);

            var result = sut.Handle(query, default);

            Assert.NotNull(result);
            Assert.Equal(result.Result.accessToken, tokensDTO.accessToken);
            Assert.Equal(result.Result.refreshToken, tokensDTO.refreshToken);
        }
        [Fact]
        public async Task RefreshTokenQueryHandler_ReturnsData()
        {
            var tokensDTO = new TokensDTO { accessToken = "accessToken", refreshToken = "refreshToken" };
            var newTokensDTO = new TokensDTO { accessToken = "accessToken2", refreshToken = "refreshToken2" };
            var loginService = new Mock<ILoginService>();
            loginService.Setup(service => service.RefreshTokens(tokensDTO)).Returns(Task.FromResult(newTokensDTO));
            var query = new RefreshTokenQuery(tokensDTO);
            var sut = new RefreshTokenQueryHandler(loginService.Object);

            var result = sut.Handle(query, default);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task RefreshTokenQueryHandler_ReturnsCorrectTokensDTO()
        {
            var tokensDTO = new TokensDTO { accessToken = "accessToken", refreshToken = "refreshToken" };
            var newTokensDTO = new TokensDTO { accessToken = "accessToken2", refreshToken = "refreshToken2" };
            var loginService = new Mock<ILoginService>();
            loginService.Setup(service => service.RefreshTokens(tokensDTO)).Returns(Task.FromResult(newTokensDTO));
            var query = new RefreshTokenQuery(tokensDTO);
            var sut = new RefreshTokenQueryHandler(loginService.Object);

            var result = sut.Handle(query, default);

            Assert.NotNull(result);
            Assert.Equal(result.Result.accessToken, newTokensDTO.accessToken);
            Assert.Equal(result.Result.refreshToken, newTokensDTO.refreshToken);
        }
        [Fact]
        public async Task RefreshTokenQueryHandler_RefreshesTokens()
        {
            var tokensDTO = new TokensDTO { accessToken = "accessToken", refreshToken = "refreshToken" };
            var newTokensDTO = new TokensDTO { accessToken = "accessToken2", refreshToken = "refreshToken2" };
            var loginService = new Mock<ILoginService>();
            loginService.Setup(service => service.RefreshTokens(tokensDTO)).Returns(Task.FromResult(newTokensDTO));
            var query = new RefreshTokenQuery(tokensDTO);
            var sut = new RefreshTokenQueryHandler(loginService.Object);

            var result = sut.Handle(query, default);

            Assert.NotNull(result);
            Assert.NotEqual(result.Result.accessToken, tokensDTO.accessToken);
            Assert.NotEqual(result.Result.refreshToken, tokensDTO.refreshToken);
        }
    }
}
