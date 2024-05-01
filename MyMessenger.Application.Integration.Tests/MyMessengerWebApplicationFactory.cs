using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using MyMessenger.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace MyMessenger.Application.Integration.Tests
{
    public class MyMessengerWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            builder.ConfigureTestServices(services =>
            {
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(configurationBuilder.GetConnectionString("DefaultConnection"));
                });
            });
        }
    }
}
