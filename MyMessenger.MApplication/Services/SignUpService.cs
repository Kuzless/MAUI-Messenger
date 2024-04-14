using Microsoft.AspNetCore.Identity;
using MyMessenger.Domain.Entities;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.AuthDTOs;
using MyMessenger.MApplication.Services.Interfaces;

namespace MyMessenger.MApplication.Services
{
    public class SignUpService : ISignUpService
    {
        private UserManager<User> userManager;
        public SignUpService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<ResponseDTO> SignUp(SignUpDTO user)
        {
            User newuser = new User() { Email = user.Email, Name = user.Name, UserName = user.UserName };
            var result = await userManager.CreateAsync(newuser, user.Password);
            if (result.Succeeded)
            {
                return new ResponseDTO { isSuccessful = true, message = "User signed up successfully." };
            } else
            {
                return new ResponseDTO { isSuccessful = false, message = "Failed to sign up." };
            }
        }
    }
}
