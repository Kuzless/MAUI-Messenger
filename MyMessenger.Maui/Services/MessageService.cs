using Blazored.LocalStorage;
using MyMessenger.Maui.Library.Interface;
using MyMessenger.Application.DTO.MessagesDTOs;

namespace MyMessenger.Maui.Services
{
    public class MessageService : GenericService<MessageDTO>
    {
        private readonly IHttpWrapper httpWrapper;
        private readonly ILocalStorageService storage;
        public MessageService(IHttpWrapper httpWrapper, ILocalStorageService storage) : base(httpWrapper, storage)
        {
        }
        public async Task DeleteMessage(int id)
        {
            var accessToken = await storage.GetItemAsStringAsync("accessToken");
            await httpWrapper.DeleteAsync($"Message/{id}", accessToken);
        }
    }
}