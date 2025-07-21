using Blazored.LocalStorage;
using MyMessenger.Maui.Models;
using MyMessenger.Maui.Library.Interface;
using System.Net.Http.Json;

namespace MyMessenger.Maui.Services
{
    public class GenericService<T> where T : class
    {
        protected readonly IHttpWrapper httpWrapper;
        protected readonly ILocalStorageService storage;
        public GenericService(IHttpWrapper httpWrapper, ILocalStorageService storage)
        {
            this.httpWrapper = httpWrapper;
            this.storage = storage;
        }
        public async Task<DataForGridDTO<T>> GetAll(AllDataRetrievalParametersDTO data, string endpoint)
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
                var response = await httpWrapper.GetAsync($"{endpoint}?{queryString}", accessToken);

                var result = await response.Content.ReadFromJsonAsync<DataForGridDTO<T>>();
                return result;
            }
            catch (Exception ex)
            {
                DataForGridDTO<T> result = new DataForGridDTO<T>() { Data = Array.Empty<T>(), NumberOfPages = 1 };
                return result;
            }
        }
    }
}
