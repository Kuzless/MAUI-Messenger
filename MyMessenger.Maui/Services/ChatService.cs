using Blazored.LocalStorage;
using MyMessenger.Maui.Models;
using MyMessenger.Maui.Library.Interface;
using System.Text.Json;

namespace MyMessenger.Maui.Services
{
    public class ChatService : GenericService<ChatDTO>
    {
        public ChatService(IHttpWrapper httpWrapper, ILocalStorageService storage) : base (httpWrapper, storage)
        {
        }
        public async Task AddChat(ChatDTO chat)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            var json = JsonSerializer.Serialize(chat);
            var response = await httpWrapper.PostAsync($"Chat", json, accessToken);
        }
        public async Task InviteToChat(ChatDTO chat, string username)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            var json = JsonSerializer.Serialize(chat);
            var response = await httpWrapper.PostAsync($"Chat/member/{username}", json, accessToken);
        }
        public async Task LeaveChat(int chatId)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            var response = await httpWrapper.DeleteAsync($"Chat/member/{chatId}", accessToken);
        }
        public async Task DeleteChat(int id)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            await httpWrapper.DeleteAsync($"Chat/{id}", accessToken);
        }
    }
}
