using Microsoft.Identity.Client.Extensions.Msal;
using MyMessenger.Domain.Entities;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.UserDTOs;
using MyMessenger.Maui.Library.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyMessenger.Maui.Services
{
    public class UserService
    {
        private readonly IHttpWrapper httpWrapper;
        private readonly ILocalStorageService storage;
        public UserService(IHttpWrapper httpWrapper, ILocalStorageService storage)
        {
           this.httpWrapper = httpWrapper;
           this.storage = storage;
        }
        public async Task<DataForGridDTO<UserDTO>>? GetAllUsers(AllDataRetrievalParametersDTO data)
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
                var response = await httpWrapper.GetAsync($"User?{queryString}", accessToken);

                var users = await response.Content.ReadFromJsonAsync<DataForGridDTO<UserDTO>>();
                return users;
            }
            catch (Exception ex)
            {
                DataForGridDTO<UserDTO> users = new DataForGridDTO<UserDTO>() { Data = Array.Empty<UserDTO>(), NumberOfPages = 1 };
                return users;
            }
        }
    }
}
