using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Domain;
using MyMessenger.Domain.Entities;
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
        private readonly DatabaseContext context;
        public MessageControllerTests(MyMessengerWebApplicationFactory factory) : base()
        {
            this.factory = factory;
            client = factory.CreateClient();
            context = factory.databaseContext;
        }
        [Fact]
        public async Task GetMessagesByChatId_ReturnsData()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Message/{1}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetMessagesByChatId_WhenIdNotSpecified_ReturnsData()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Message/{0}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetMessagesByChatId_ReturnsMessages()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Message/{1}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
        }
        [Fact]
        public async Task GetMessagesByChatId_WhenIdNotSpecified_ReturnsMessages()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Message/{0}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
        }
        [Fact]
        public async Task GetMessagesByChatId_ReturnsMessagesFromSameChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Message/{1}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

            Assert.NotNull(result);
            Assert.True(result.Data.All(message => message.ChatId == 1));
        }
        [Fact]
        public async Task GetMessagesByChatId_WhenIdNotSpecified_ReturnsAllUserMessages()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Message/{0}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();

            Assert.NotNull(result);
            Assert.True(result.Data.All(message => message.Name == "IntegrationTestsUser"));
        }
        [Fact]
        public async Task SendMessage_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            MessageDTO message = new MessageDTO() { DateTime = DateTime.Now, ChatId = 1, Text = "CreatedMessage" };

            var json = JsonSerializer.Serialize(message);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/Message", httpContent);

            //Assert
            var addedMessage = context.Messages.AsNoTracking().FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text);

            response.EnsureSuccessStatusCode();

            //CleanUp

            if (addedMessage != null)
            {
                context.Messages.Remove(addedMessage);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task SendMessage_AddsMessageToChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            MessageDTO message = new MessageDTO() { DateTime = DateTime.Now, ChatId = 1, Text = "CreatedMessage" };

            var json = JsonSerializer.Serialize(message);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/Message", httpContent);

            //Assert
            var addedMessage = context.Messages.AsNoTracking().FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(addedMessage);

            //CleanUp

            if (addedMessage != null)
            {
                context.Messages.Remove(addedMessage);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task UpdateMessage_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            Message oldMessage = context.Messages.FirstOrDefault(m => m.Id == 4);
            MessageDTO message = new MessageDTO() { DateTime = DateTime.Now, ChatId = 1, Text = "UpdatedMessage", Id = 4 };

            var json = JsonSerializer.Serialize(message);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("/api/Message", httpContent);

            //Assert
            Message addedMessage = context.Messages.AsNoTracking().FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text);

            response.EnsureSuccessStatusCode();

            //CleanUp
            if (addedMessage != null)
            {
                context.Messages.Update(oldMessage);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task UpdateMessage_WhenUserIsOwner_UpdatesMessage()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            Message oldMessage = context.Messages.FirstOrDefault(m => m.Id == 4);
            MessageDTO message = new MessageDTO() { DateTime = DateTime.Now, ChatId = 1, Text = "UpdatedMessage", Id = 4 };

            var json = JsonSerializer.Serialize(message);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("/api/Message", httpContent);

            //Assert
            Message addedMessage = context.Messages.AsNoTracking().FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text);

            response.EnsureSuccessStatusCode();
            Assert.False(oldMessage.Equals(addedMessage));

            //CleanUp
            if (addedMessage != null)
            {
                context.Messages.Update(oldMessage);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task UpdateMessage_WhenUserIsNotOwner_DoesntUpdatesMessage()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            Message oldMessage = context.Messages.FirstOrDefault(m => m.Id == 5);
            MessageDTO message = new MessageDTO() { DateTime = DateTime.Now, ChatId = 1, Text = "UpdatedMessage", Id = 5 };

            var json = JsonSerializer.Serialize(message);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("/api/Message", httpContent);

            //Assert
            Message addedMessage = context.Messages.AsNoTracking().FirstOrDefault(m => m.ChatId == oldMessage.ChatId && m.Text == oldMessage.Text);

            response.EnsureSuccessStatusCode();
            Assert.True(oldMessage.Text == addedMessage.Text);

            //CleanUp
            if (addedMessage != null)
            {
                context.Messages.Update(oldMessage);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task DeleteMessage_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            var dateTime = DateTime.Now;
            Message message = new Message() { DateTime = dateTime, ChatId = 1, Text = "MessageToDelete", UserId = "e13fa002-a86a-419a-a829-6efb0e000c70" };
            context.Messages.Add(message);
            await context.SaveChangesAsync();
            var messageToDelete = context.Messages.FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text && m.DateTime == dateTime);

            //Act
            var response = await client.DeleteAsync($"/api/Message/{messageToDelete.Id}");

            //Assert
            var deletedMessage = context.Messages.FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text && m.DateTime == dateTime);

            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task DeleteMessage_WhenUserIsOwner_DeletesMessage()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            var dateTime = DateTime.Now;
            Message message = new Message() { DateTime = dateTime, ChatId = 1, Text = "MessageToDelete", UserId = "e13fa002-a86a-419a-a829-6efb0e000c70" };
            context.Messages.Add(message);
            await context.SaveChangesAsync();
            var messageToDelete = context.Messages.FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text && m.DateTime == dateTime);

            //Act
            var response = await client.DeleteAsync($"/api/Message/{messageToDelete.Id}");

            //Assert
            var deletedMessage = context.Messages.FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text && m.DateTime == dateTime);

            response.EnsureSuccessStatusCode();
            Assert.Null(deletedMessage);
        }
        [Fact]
        public async Task DeleteMessage_WhenUserIsNotOwner_DoesntDeletesMessage()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            var dateTime = DateTime.Now;
            Message message = new Message() { DateTime = dateTime, ChatId = 1, Text = "MessageToDelete", UserId = "f7853dc7-6aaa-4f8f-873e-f30e2815b713" };
            context.Messages.Add(message);
            await context.SaveChangesAsync();
            var messageToDelete = context.Messages.FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text && m.DateTime == dateTime);

            //Act
            var response = await client.DeleteAsync($"/api/Message/{messageToDelete.Id}");

            //Assert
            var deletedMessage = context.Messages.FirstOrDefault(m => m.ChatId == message.ChatId && m.Text == message.Text && m.DateTime == dateTime);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(deletedMessage);

            //CleanUp

            if (deletedMessage != null)
            {
                context.Messages.Remove(deletedMessage);
                await context.SaveChangesAsync();
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
