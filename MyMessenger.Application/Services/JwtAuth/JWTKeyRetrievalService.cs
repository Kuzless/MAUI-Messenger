using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using System.Text;

namespace MyMessenger.Application.Services.JwtAuth
{
    public class JWTKeyRetrievalService : IJWTKeyRetrievalService
    {
        private readonly IConfiguration configuration;
        public JWTKeyRetrievalService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public SymmetricSecurityKey GetJwtSecretKey()
        {
            if (!(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"))
            {
                var keyVaultUrl = configuration["Keyvault:KeyvaultUrl"];
                var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(client.GetSecret("JWTSecretKey").Value.Value));
            }
            else
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]));
            }
        }
    }
}
