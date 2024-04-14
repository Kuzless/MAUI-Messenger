using Blazored.LocalStorage;
using MyMessenger.MApplication.DTO;
using MyMessenger.Maui.Library.Interface;
using System.Net.Http.Json;
using MyMessenger.MApplication.DTO.MessagesDTOs;

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
    }
}