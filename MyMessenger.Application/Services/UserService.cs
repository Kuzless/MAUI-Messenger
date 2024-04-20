using Microsoft.AspNetCore.Identity;
using MyMessenger.Domain.Entities;
using MyMessenger.Application.Services.Interfaces;

namespace MyMessenger.Application.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> userManager;
        public UserService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<User> GetUserByUserName(string name)
        {
            var user = await userManager.FindByNameAsync(name);
            return user;
        }
    }
}
