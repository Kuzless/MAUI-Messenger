using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyMessenger.Domain;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using MyMessenger.Domain.Repositories;
using MyMessenger.Application.Services;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Application.Services.JwtAuth;
using MyMessenger.Application.Services.JwtAuth.Interfaces;
using MyMessenger.Application.ÑommandsQueries.Users.Queries;
using MyMessenger.Options;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyMessenger.HubConfig;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Storage.Blobs;

namespace MyMessenger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string secretKey = "";
            string blobConnection = "";
            builder.Services.AddControllers();

            // 

            if (builder.Environment.IsProduction())
            {
                var keyVault = builder.Configuration.GetSection("Keyvault:KeyvaultUrl");
                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
                builder.Configuration.AddAzureKeyVault(keyVault.Value!.ToString(), new DefaultKeyVaultSecretManager());
                var client = new SecretClient(new Uri(keyVault.Value!.ToString()), new DefaultAzureCredential());
                builder.Services.AddDbContext<DatabaseContext>(options => {
                    options.UseSqlServer(client.GetSecret("DatabaseConnectionString").Value.Value.ToString());
                    options.UseLazyLoadingProxies();
                });
                secretKey = client.GetSecret("JWTSecretKey").Value.Value.ToString();
                blobConnection = client.GetSecret("BlobConnectionString").Value.Value.ToString();
            }
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<DatabaseContext>(options => {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                    options.UseLazyLoadingProxies();
                });
                secretKey = builder.Configuration["Jwt:SecretKey"];
                blobConnection = builder.Configuration["Keyvault:BlobConnection"];
            }

            //

            builder.Services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.User.RequireUniqueEmail = true;
                    })
                .AddTokenProvider<UserAuthProvider<User>>("MyMessenger")
                .AddEntityFrameworkStores<DatabaseContext>();

            //

            builder.Services.AddAutoMapper(typeof(AutoMappingProfile)); 
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISignUpService, SignUpService>();
            builder.Services.AddScoped<IJWTGeneratorService, JWTGeneratorService>();
            builder.Services.AddScoped<ITokenValidatorService, TokenValidatorService>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
            builder.Services.AddScoped<IJWTKeyRetrievalService, JWTKeyRetrievalService>();
            builder.Services.AddSingleton(x => new BlobServiceClient(blobConnection));

            //

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginQueryHandler).Assembly));

            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                        builder =>
                        {
                            builder
                            .WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapHub<ChatHub>("/chathub");
            app.MapControllers().RequireCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
