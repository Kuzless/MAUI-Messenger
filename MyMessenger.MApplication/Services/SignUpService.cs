using Microsoft.AspNetCore.Identity;
using MyMessenger.Domain.Entities;
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
        public async Task<SignUpResponseDTO> SignUp(SignUpDTO user)
        {
            User newuser = new User() { Email = user.Email, Name = user.Name, UserName = user.UserName };
            var result = await userManager.CreateAsync(newuser, user.Password);
            if (result.Succeeded)
            {
                return new SignUpResponseDTO { isSuccessful = true, message = "User signed up successfully." };
            } else
            {
                return new SignUpResponseDTO { isSuccessful = false, message = "Failed to sign up." };
            }
        }
    }
}
