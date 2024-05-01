using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using MyMessenger.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace MyMessenger.Application.Integration.Tests
{
    public class MyMessengerWebApplicationFactory : WebApplicationFactory<Program>
    {
        internal DatabaseContext databaseContext;
        private readonly string connectionString;
        public MyMessengerWebApplicationFactory()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            connectionString = configurationBuilder.GetConnectionString("TestConnection");
            optionsBuilder.UseSqlServer(connectionString);
            databaseContext = new DatabaseContext(optionsBuilder.Options);
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var configuration = config.Build();

                var testConnectionString = configuration.GetConnectionString("TestConnection");

                configuration["ConnectionStrings:DefaultConnection"] = testConnectionString;
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            });
        }
    }
}
