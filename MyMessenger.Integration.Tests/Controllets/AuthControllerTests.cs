using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.Integration.Tests.Configuration;
using MyMessenger.Domain;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MyMessenger.Application.Integration.Tests.Controllets
{
    public class AuthControllerTests : IClassFixture<MyMessengerWebApplicationFactory>
    {
        private readonly HttpClient client;
        private readonly DatabaseContext context;
        public AuthControllerTests(MyMessengerWebApplicationFactory factory) : base()
        {
            client = factory.CreateClient();
            context = factory.databaseContext;
        }
        [Fact]
        public async Task Login_ReturnsSuccess()
        {
            // Arrange
            var email = "IntegrationTestsUser@IntegrationTestsUser.com";
            var password = "123QWEasd!";
            var json = JsonSerializer.Serialize(new { email, password });
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Auth/", httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Login_ReturnsData()
        {
            // Arrange
            var email = "IntegrationTestsUser@IntegrationTestsUser.com";
            var password = "123QWEasd!";
            var json = JsonSerializer.Serialize(new { email, password });
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Auth/", httpContent);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<TokensDTO>();

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
        }
        [Theory]
        [InlineData("IntegrationTestsUser@IntegrationTestsUser.com", "123QWEasd!")]
        [InlineData("IntegrationTestsUser2@IntegrationTestsUser2.com", "123QWEasd!")]
        public async Task Login_WhenCorrectCredentials_ReturnsTokens(string email, string password)
        {
            // Arrange
            var json = JsonSerializer.Serialize(new { email, password });
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Auth/", httpContent);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<TokensDTO>();

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<TokensDTO>(result);
        }
        [Theory]
        [InlineData("IntegrationTestsUser@IntegrationTestsUser1.com", "123QWEasd!")]
        [InlineData("IntegrationTestsUser2@IntegrationTestsUser.com", "123QWEasd")]
        [InlineData("IntegrationTestsUser2@IntegrationTestsUser2.com", "123QWEasd!!")]
        public async Task Login_WhenInvalidCredentials_ReturnsBadRequest(string email, string password)
        {
            // Arrange
            var json = JsonSerializer.Serialize(new { email, password });
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Auth/", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Refresh_ReturnsSuccess()
        {
            // Arrange
            var tokens = await Authorize("IntegrationTestsUser@IntegrationTestsUser.com", "123QWEasd!");
            await Task.Delay(2000);

            var json = JsonSerializer.Serialize(tokens);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("api/Auth/refresh", httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Refresh_ReturnsData()
        {
            // Arrange
            var tokens = await Authorize("IntegrationTestsUser@IntegrationTestsUser.com", "123QWEasd!");
            await Task.Delay(2500);

            var json = JsonSerializer.Serialize(tokens);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("api/Auth/refresh", httpContent);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<TokensDTO>();

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
        }
        [Theory]
        [InlineData("IntegrationTestsUser2@IntegrationTestsUser2.com", "123QWEasd!")]
        [InlineData("IntegrationTestsUser@IntegrationTestsUser.com", "123QWEasd!")]
        public async Task Refresh_WhenCorrectTokens_ReturnsNewTokens(string email, string password)
        {
            // Arrange
            var tokens = await Authorize(email, password);
            await Task.Delay(3000);

            var json = JsonSerializer.Serialize(tokens);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("api/Auth/refresh", httpContent);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<TokensDTO>();

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(tokens.accessToken, result.accessToken);
            Assert.NotEqual(tokens.refreshToken, result.refreshToken);
        }
        [Fact]
        public async Task Sign_ReturnsSuccess()
        {
            // Arrange
            var user = new SignUpDTO
            {
                Name = "User1",
                UserName = "user1",
                Email = "user1@gmail.com",
                Password = "123QWEasd!"
            };
            var json = JsonSerializer.Serialize(user);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Auth/sign", httpContent);

            // Assert
            var addedUser = context.Users.FirstOrDefault(u => u.Name == user.Name && u.UserName == user.UserName && u.Email == user.Email);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //CleanUp

            if (addedUser != null)
            {
                context.Users.Remove(addedUser);
                await context.SaveChangesAsync();
            }
        }
        [Theory]
        [InlineData("User1", "user1", "user1@gmail.com")]
        [InlineData("User2", "user2", "user2@gmail.com")]
        [InlineData("User3", "user3", "user3@gmail.com")]
        public async Task Sign_IfCorrectCredentials_CreatesNewUser(string name, string username, string email)
        {
            // Arrange
            var user = new SignUpDTO
            {
                Name = name,
                UserName = username,
                Email = email,
                Password = "123QWEasd!"
            };
            var json = JsonSerializer.Serialize(user);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Auth/sign", httpContent);

            // Assert
            var addedUser = context.Users.FirstOrDefault(u => u.Name == name && u.UserName == username && u.Email == email);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(addedUser);

            //CleanUp

            if (addedUser != null)
            {
                context.Users.Remove(addedUser);
                await context.SaveChangesAsync();
            }
        }
        [Theory]
        [InlineData("123QWEasd")]
        [InlineData("123qweasd!")]
        [InlineData("QWEasd!!@#")]
        public async Task Sign_IfIncorrectCredentials_ReturnsFailture(string password)
        {
            // Arrange
            var user = new SignUpDTO
            {
                Name = "User1",
                UserName = "user1",
                Email = "user1@gmail.com",
                Password = password
            };
            var json = JsonSerializer.Serialize(user);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Auth/sign", httpContent);

            // Assert
            var addedUser = context.Users.FirstOrDefault(u => u.Name == user.Name && u.UserName == user.UserName && u.Email == user.Email);
            var content = await response.Content.ReadFromJsonAsync<ResponseDTO>();

            Assert.False(content.isSuccessful);
            Assert.Null(addedUser);

            //CleanUp

            if (addedUser != null)
            {
                context.Users.Remove(addedUser);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task SignUp_IfUserExists_ReturnsFailture()
        {
            // Arrange
            var user = new SignUpDTO
            {
                Name = "existingUser",
                UserName = "existingUser",
                Email = "existinguser@gmail.com",
                Password = "123QWEasd!"
            };
            var json = JsonSerializer.Serialize(user);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            await client.PostAsync("/api/Auth/sign", httpContent);
            var response = await client.PostAsync("/api/Auth/sign", httpContent);

            // Assert
            var addedUser = context.Users.FirstOrDefault(u => u.Name == user.Name && u.UserName == user.UserName && u.Email == user.Email);
            var content = await response.Content.ReadFromJsonAsync<ResponseDTO>();

            Assert.False(content.isSuccessful);

            //CleanUp

            if (addedUser != null)
            {
                context.Users.Remove(addedUser);
                await context.SaveChangesAsync();
            }
        }
        private async Task<TokensDTO> Authorize(string email, string password)
        {
            var loginModel = new { Email = email, Password = password };
            var tokenResponse = await client.PostAsJsonAsync("/api/Auth", loginModel);
            var tokens = await tokenResponse.Content.ReadFromJsonAsync<TokensDTO>();
            return tokens;
        }
    }
}
