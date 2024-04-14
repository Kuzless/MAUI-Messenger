using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyMessenger.Application.Services.JwtAuth;
using System.Text;

namespace MyMessenger.Options
{
    public class JWTBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
    {
        private readonly JWTOptions jwtoptions;
        public JWTBearerOptionsSetup(IOptions<JWTOptions> jwtoptions)
        {
            this.jwtoptions = jwtoptions.Value;
        }
        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtoptions.Issuer,
                ValidAudience = jwtoptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtoptions.SecretKey))
            };
        }
    }
}
