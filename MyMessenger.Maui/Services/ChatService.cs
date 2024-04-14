using Blazored.LocalStorage;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.ChatDTOs;
using MyMessenger.Maui.Library.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<DataForGridDTO<ChatDTO>>? GetAllMessages(AllDataRetrievalParametersDTO data)
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
    }
}
