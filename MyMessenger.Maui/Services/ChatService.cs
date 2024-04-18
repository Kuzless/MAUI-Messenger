using Blazored.LocalStorage;
using Microsoft.AspNetCore.Http;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.ChatDTOs;
using MyMessenger.Maui.Library.Interface;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyMessenger.Maui.Services
{
    public class ChatService
    {
        private readonly IHttpWrapper httpWrapper;
        private readonly ILocalStorageService storage;
        public ChatService(IHttpWrapper httpWrapper, ILocalStorageService storage)
        {
            this.httpWrapper = httpWrapper;
            this.storage = storage;
        }
        public async Task<DataForGridDTO<ChatDTO>>? GetAllChats(AllDataRetrievalParametersDTO data)
        {
            var queryString = $"PageNumber={data.PageNumber}&PageSize={data.PageSize}";

            if (!string.IsNullOrEmpty(data.Subs))
            {
                queryString += $"&Subs={Uri.EscapeDataString(data.Subs)}";
            }

            if (data.Sort != null && data.Sort.Count > 0)
            {
                foreach (var (key, value) in data.Sort)
                {
                    queryString += $"&Sort[{Uri.EscapeDataString(key)}]={value}";
                }
            }
            try
            {
                var accessToken = await storage.GetItemAsStringAsync("accessToken");
                var response = await httpWrapper.GetAsync($"Chat?{queryString}", accessToken);

                var users = await response.Content.ReadFromJsonAsync<DataForGridDTO<ChatDTO>>();
                return users;
            }
            catch (Exception ex)
            {
                DataForGridDTO<ChatDTO> users = new DataForGridDTO<ChatDTO>() { Data = Array.Empty<ChatDTO>(), NumberOfPages = 1 };
                return users;
            }
        }
        public async Task AddChat(ChatDTO chat)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            var json = JsonSerializer.Serialize(chat);
            var response = await httpWrapper.PostAsync($"Chat", json, accessToken);
        }
        public async Task DeleteChat(int id)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            await httpWrapper.DeleteAsync($"Chat/{id}", accessToken);
        }
    }
}
