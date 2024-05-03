using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using MyMessenger.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace MyMessenger.Application.Integration.Tests.Configuration
{
    public class MyMessengerWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
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

                configuration["ConnectionStrings:DefaultConnection"] = connectionString;
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            });
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                databaseContext.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
