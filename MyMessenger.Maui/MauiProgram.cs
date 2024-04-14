using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MyMessenger.Maui.Library;
using MyMessenger.Maui.Library.Interface;
using MyMessenger.Maui.Services;
using Syncfusion.Blazor;

namespace MyMessenger.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddScoped<IHttpWrapper, HttpWrapper>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<MessageService>();
            builder.Services.AddScoped<ChatService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddAuthorization();
            builder.Services.AddAuthorizationCore();
            builder.Services.TryAddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddSingleton<CustomAuthService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
