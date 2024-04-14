using Microsoft.Extensions.Options;
using MyMessenger.Application.Services.JwtAuth;

namespace MyMessenger.Options
{
    public class JWTOptionsSetup : IConfigureOptions<JWTOptions>
    {
        private readonly IConfiguration configuration;
        public JWTOptionsSetup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void Configure(JWTOptions options)
        {
            configuration.GetSection("Jwt").Bind(options);
        }
    }
}
