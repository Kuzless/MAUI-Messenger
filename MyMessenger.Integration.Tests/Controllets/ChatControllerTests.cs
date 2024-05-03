using Microsoft.AspNetCore.Mvc.Testing;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.DTO;
using MyMessenger.Application.Integration.Tests.Configuration;
using MyMessenger.Domain;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MyMessenger.Application.DTO.AuthDTOs;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MyMessenger.Application.DTO.ChatDTOs;
using MyMessenger.Domain.Entities;

namespace MyMessenger.Application.Integration.Tests.Controllets
{
    public class ChatControllerTests : IClassFixture<MyMessengerWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly HttpClient client;
        private readonly DatabaseContext context;
        public ChatControllerTests(MyMessengerWebApplicationFactory factory) : base()
        {
            this.factory = factory;
            client = factory.CreateClient();
            context = factory.databaseContext;
        }
        [Fact]
        public async Task GetAllChats_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Chat/");

            //Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task GetAllChats_ReturnsData()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Chat/");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<ChatDTO>>();

            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllChats_ReturnsChats()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Chat/");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<ChatDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
        }
        [Fact]
        public async Task GetAllChats_ReturnsCorrectNumberOfChats()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            //Act
            var response = await client.GetAsync($"/api/Chat/");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<ChatDTO>>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Count() == 1);
        }
        [Fact]
        public async Task CreateChat_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            ChatDTO chat = new ChatDTO() { Name = "CreatedChat" };

            var json = JsonSerializer.Serialize(chat);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/Chat", httpContent);

            //Assert
            var addedChat = context.Chats.AsNoTracking().FirstOrDefault(c => c.Name == chat.Name);

            response.EnsureSuccessStatusCode();

            //CleanUp

            if (addedChat != null)
            {
                context.Chats.Remove(addedChat);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task CreateChat_AddsChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            ChatDTO chat = new ChatDTO() { Name = "CreatedChat" };

            var json = JsonSerializer.Serialize(chat);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/Chat", httpContent);

            //Assert
            var addedChat = context.Chats.AsNoTracking().FirstOrDefault(c => c.Name == chat.Name);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(addedChat);

            //CleanUp

            if (addedChat != null)
            {
                context.Chats.Remove(addedChat);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task JoinChat_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            Chat chat = new Chat() { Name = "ChatToInvite", OwnerId = "e13fa002-a86a-419a-a829-6efb0e000c70" };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            var chatToInvite = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            var json = JsonSerializer.Serialize(chatToInvite);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync($"/api/Chat/member/IntegrationTestsUser2", httpContent);

            //Assert
            response.EnsureSuccessStatusCode();

            //CleanUp
            if (chatToInvite != null)
            {
                context.Chats.Remove(chatToInvite);
                await context.SaveChangesAsync();
            }

        }
        [Fact]
        public async Task JoinChat_AddsMemberToChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            Chat chat = new Chat() { Name = "ChatToInvite", OwnerId = "e13fa002-a86a-419a-a829-6efb0e000c70" };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            var chatToInvite = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            var json = JsonSerializer.Serialize(chatToInvite);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync($"/api/Chat/member/IntegrationTestsUser2", httpContent);

            //Assert
            chatToInvite = context.Chats.Include(c => c.Users).FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            response.EnsureSuccessStatusCode();
            Assert.True(chatToInvite.Users.Count() == 1);

            //CleanUp
            if (chatToInvite != null)
            {
                context.Chats.Remove(chatToInvite);
                await context.SaveChangesAsync();
            }

        }
        [Fact]
        public async Task DeleteChat_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            Chat chat = new Chat() { Name = "ChatToDelete", OwnerId = "e13fa002-a86a-419a-a829-6efb0e000c70" };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            var chatToDelete = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            //Act
            var response = await client.DeleteAsync($"/api/Chat/{chatToDelete.Id}");

            //Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task DeleteChat_WhenUserIsOwner_DeletesChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            Chat chat = new Chat() { Name = "ChatToDelete", OwnerId = "e13fa002-a86a-419a-a829-6efb0e000c70" };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            var chatToDelete = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            //Act
            var response = await client.DeleteAsync($"/api/Chat/{chatToDelete.Id}");

            //Assert
            chatToDelete = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            response.EnsureSuccessStatusCode();
            Assert.Null(chatToDelete);

            //CleanUp

            if (chatToDelete != null)
            {
                context.Chats.Remove(chatToDelete);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task DeleteChat_WhenUserIsNotOwner_DoesntDeletesChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            Chat chat = new Chat() { Name = "ChatToDelete", OwnerId = "f7853dc7-6aaa-4f8f-873e-f30e2815b713" };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            var chatToDelete = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            //Act
            var response = await client.DeleteAsync($"/api/Chat/{chatToDelete.Id}");

            //Assert
            chatToDelete = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(chatToDelete);

            //CleanUp

            if (chatToDelete != null)
            {
                context.Chats.Remove(chatToDelete);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task LeaveChat_ReturnsSuccess()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            var owner = context.Users.FirstOrDefault(u => u.Id == "e13fa002-a86a-419a-a829-6efb0e000c70");
            var member = context.Users.FirstOrDefault(u => u.Id == "f7853dc7-6aaa-4f8f-873e-f30e2815b713");
            Chat chat = new Chat() { Name = "ChatToLeave", OwnerId = "e13fa002-a86a-419a-a829-6efb0e000c70", Users = { owner, member } };

            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            
            var chatToLeave = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            //Act
            var response = await client.DeleteAsync($"/api/Chat/member/{chatToLeave.Id}");

            //Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task LeaveChat_RemovesUserFromChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            var member = context.Users.FirstOrDefault(u => u.Id == "e13fa002-a86a-419a-a829-6efb0e000c70");
            var owner = context.Users.FirstOrDefault(u => u.Id == "f7853dc7-6aaa-4f8f-873e-f30e2815b713");
            Chat chat = new Chat() { Name = "ChatToLeave", OwnerId = "f7853dc7-6aaa-4f8f-873e-f30e2815b713", Users = { owner, member } };

            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            context.Entry(member).State = EntityState.Detached;
            context.Entry(owner).State = EntityState.Detached;
            context.Entry(chat).State = EntityState.Detached;

            var chatToLeave = context.Chats.AsNoTracking().Include(c => c.Users).FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            //Act
            var response = await client.DeleteAsync($"/api/Chat/member/{chatToLeave.Id}");

            //Assert
            var chatWithoutMember = context.Chats.AsNoTracking().Include(c => c.Users).FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            response.EnsureSuccessStatusCode();
            Assert.True(chatWithoutMember.Users.Count() == 1);

            //CleanUp
            if (chatWithoutMember != null)
            {
                context.Chats.Remove(chatWithoutMember);
                await context.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task LeaveChat_OwnerLeaves_DeleteChat()
        {
            //Arrange
            var tokens = await Authorize();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.accessToken);

            var owner = context.Users.FirstOrDefault(u => u.Id == "e13fa002-a86a-419a-a829-6efb0e000c70");
            var member = context.Users.FirstOrDefault(u => u.Id == "f7853dc7-6aaa-4f8f-873e-f30e2815b713");
            Chat chat = new Chat() { Name = "ChatToLeave", OwnerId = "e13fa002-a86a-419a-a829-6efb0e000c70", Users = { owner, member } };

            context.Chats.Add(chat);
            await context.SaveChangesAsync();


            var chatToLeave = context.Chats.FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            //Act
            var response = await client.DeleteAsync($"/api/Chat/member/{chatToLeave.Id}");

            //Assert
            chatToLeave = context.Chats.Include(c => c.Users).FirstOrDefault(c => c.Name == chat.Name && c.OwnerId == chat.OwnerId);

            response.EnsureSuccessStatusCode();
            Assert.Null(chatToLeave);

            //CleanUp
            if (chatToLeave != null)
            {
                context.Chats.Remove(chatToLeave);
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
