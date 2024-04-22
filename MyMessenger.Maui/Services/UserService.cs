using Blazored.LocalStorage;
using MyMessenger.Application.DTO.UserDTOs;
using MyMessenger.Maui.Library.Interface;

namespace MyMessenger.Maui.Services
{
    public class UserService : GenericService<UserDTO>
    {
        public UserService(IHttpWrapper httpWrapper, ILocalStorageService storage) : base(httpWrapper, storage)
        {
            
        }
    }
}
