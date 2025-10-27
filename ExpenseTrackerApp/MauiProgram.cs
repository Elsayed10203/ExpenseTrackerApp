using ExpenseTrackerApp.Helper;
using ExpenseTrackerApp.Services.IHttpClient;
using ExpenseTrackerApp.UI.Modules.Home;
using ExpenseTrackerApp.UI.Modules.Login;
using Microsoft.Extensions.Logging;
using FreshMvvm.Maui.Extensions;
using ExpenseTrackerApp.Services.Auth;


namespace ExpenseTrackerApp
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    // FontAwesome
                    fonts.AddFont("FontAwesome5Brands-Regular-400.otf", "FAPB");
                    fonts.AddFont("FontAwesome5Duotone-Solid-900.otf", "FAPD");
                    fonts.AddFont("FontAwesome5Pro-Light-300.otf", "FAPL");
                    fonts.AddFont("FontAwesome5Pro-Regular-400.otf", "FAPR");
                    fonts.AddFont("FontAwesome5Pro-Solid-900.otf", "FAPS");
                    fonts.AddFont("FontAwesome6FreeBrands400.otf", "FAFB");
                    fonts.AddFont("FontAwesome6FreeRegular400.otf", "FAFR");
                    fonts.AddFont("FontAwesome6FreeSolid.otf", "FAFS");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    AppHandlers.Init();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginPageModel>();


            builder.Services.AddTransient<TappedHomePage>();
            builder.Services.AddTransient<TappedHomePageModel>();
             

            builder.Services.AddTransient<IHttpProvider, HttpProvider>();
           
            builder.Services.AddSingleton<IAuthService, AuthService>();

            // Build app first
            var mauiApp = builder.Build();

            // Initialize FreshMvvm against the built app
            mauiApp.UseFreshMvvm();

            return builder.Build();
        }
    }
}
