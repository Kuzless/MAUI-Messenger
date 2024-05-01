using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Domain;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MyMessenger.Application.Integration.Tests
{
    public class MessageControllerTests : IClassFixture<MyMessengerWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly HttpClient client;
        public MessageControllerTests(MyMessengerWebApplicationFactory factory)
        {
            this.factory = factory;
            client = factory.CreateClient();
        }
        [Fact]
        public async Task GetMessagesByChatId_ReturnsData()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                //Act
                var response = await client.GetAsync($"/api/Message/{75}");

                //Assert
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

                Assert.NotNull(result);
            }
        }
        [Fact]
        public async Task GetMessagesByChatId_WhenIdNotSpecified_ReturnsData()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                //Act
                var response = await client.GetAsync($"/api/Message/{0}");

                //Assert
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

                Assert.NotNull(result);
            }
        }
        [Fact]
        public async Task GetMessagesByChatId_ReturnsMessages()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                //Act
                var response = await client.GetAsync($"/api/Message/{75}");

                //Assert
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

                Assert.NotNull(result);
                Assert.NotEmpty(result.Data);
            }
        }
        [Fact]
        public async Task GetMessagesByChatId_WhenIdNotSpecified_ReturnsMessages()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                //Act
                var response = await client.GetAsync($"/api/Message/{0}");

                //Assert
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

                Assert.NotNull(result);
                Assert.NotEmpty(result.Data);
            }
        }
        [Fact]
        public async Task GetMessagesByChatId_ReturnsMessagesFromSameChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                //Act
                var response = await client.GetAsync($"/api/Message/{75}");

                //Assert
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

                Assert.NotNull(result);
                Assert.True(result.Data.All(message => message.ChatId == 75));
            }
        }
        [Fact]
        public async Task GetMessagesByChatId_WhenIdNotSpecified_ReturnsAllMessages()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                //Act
                var response = await client.GetAsync($"/api/Message/{0}");

                //Assert
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

                Assert.NotNull(result);
                Assert.True(result.Data.Any(message => message.ChatId == 75));
                Assert.True(result.Data.Any(message => message.ChatId != 75));
            }
        }
        [Fact]
        public async Task SendMessage_AddsMessageToChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                MessageDTO message = new MessageDTO() { DateTime = DateTime.Now, ChatId = 43, Text = "Test123" };

                var json = JsonSerializer.Serialize(message);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                //Act
                var response = await client.PostAsync("/api/Message", httpContent);
                
                //Assert
                response.EnsureSuccessStatusCode();

                //CleanUp
                var addedMessage = context.Messages.FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text);
                if (addedMessage != null)
                {
                    context.Messages.Remove(addedMessage);
                    await context.SaveChangesAsync();
                }
            }
        }
        [Fact]
        public async Task UpdateMessage_UpdatesMessage()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var oldMessage = context.Messages.FirstOrDefault(m => m.Id == 213);
                MessageDTO message = new MessageDTO() { DateTime = DateTime.Now, ChatId = 43, Text = "Test123", Id = 213 };

                var json = JsonSerializer.Serialize(message);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                //Act
                var response = await client.PutAsync("/api/Message", httpContent);

                //Assert
                response.EnsureSuccessStatusCode();

                //CleanUp
                var addedMessage = context.Messages.FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text);
                if (addedMessage != null)
                {
                    context.Messages.Update(oldMessage);
                    await context.SaveChangesAsync();
                }
            }
        }
        private async Task<TokensDTO> Authorize()
        {
            var loginModel = new { Email = "IntegrationTestsUser@IntegrationTestsUser.com", Password = "123QWEasd!" };
            var tokenResponse = await client.PostAsJsonAsync("/api/Auth", loginModel);
            tokenResponse.EnsureSuccessStatusCode();
            var tokens = await tokenResponse.Content.ReadFromJsonAsync<TokensDTO>();
            return tokens;
        }
    }       
}
