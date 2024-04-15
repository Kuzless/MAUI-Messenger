using Blazored.LocalStorage;
using MyMessenger.Application.DTO;
using MyMessenger.Maui.Library.Interface;
using System.Net.Http.Json;
using MyMessenger.Application.DTO.MessagesDTOs;
using System.Text.Json;

namespace MyMessenger.Maui.Services
{
    public class MessageService
    {
        private readonly IHttpWrapper httpWrapper;
        private readonly ILocalStorageService storage;
        public MessageService(IHttpWrapper httpWrapper, ILocalStorageService storage)
        {
            this.httpWrapper = httpWrapper;
            this.storage = storage;
        }
        public async Task<DataForGridDTO<MessageDTO>>? GetAllMessages(AllDataRetrievalParametersDTO data)
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
                var response = await httpWrapper.GetAsync($"Message?{queryString}", accessToken);

                var users = await response.Content.ReadFromJsonAsync<DataForGridDTO<MessageDTO>>();
                return users;
            }
            catch (Exception ex)
            {
                DataForGridDTO<MessageDTO> users = new DataForGridDTO<MessageDTO>() { Data = Array.Empty<MessageDTO>(), NumberOfPages = 1 };
                return users;
            }
        }
        public async Task AddMessage(MessageDTO message)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            var json = JsonSerializer.Serialize(message);
            var response = await httpWrapper.PostAsync($"Message", json, accessToken);
        }
        public async Task DeleteMessage(int id)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            await httpWrapper.DeleteAsync($"Message/{id}", accessToken);
        }
    }
}