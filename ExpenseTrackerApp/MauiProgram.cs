using ExpenseTrackerApp.UI.Modules.Login;
using Microsoft.Extensions.Logging;

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
                });

                
#if DEBUG
            builder.Logging.AddDebug();
#endif
           


            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginPageModel>();

            return builder.Build();
        }
    }
}
