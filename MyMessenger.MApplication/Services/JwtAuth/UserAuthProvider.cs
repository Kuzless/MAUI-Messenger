using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace MyMessenger.MApplication.Services.JwtAuth
{
    public class UserAuthProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
    {
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(true);
        }

        public Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rng = new RNGCryptoServiceProvider();
            var randomBytes = new byte[32];
            rng.GetBytes(randomBytes);

            var chars = new char[32];
            var numAllowedChars = allowedChars.Length;

            for (int i = 0; i < 32; i++)
            {
                chars[i] = allowedChars[randomBytes[i] % numAllowedChars];
            }

            string token = new string(chars);
            return Task.FromResult(token);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(true);
        }
    }
}
