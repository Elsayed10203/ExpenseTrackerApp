using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using DevExpress.Maui;
using ExpenseTrackerApp.Helper;
using ExpenseTrackerApp.Services;
using ExpenseTrackerApp.Services.Auth;
using ExpenseTrackerApp.Services.Expense;
using ExpenseTrackerApp.Services.IHttpClient;
using ExpenseTrackerApp.UI.Modules.AddEditExpense;
using ExpenseTrackerApp.UI.Modules.Category;
using ExpenseTrackerApp.UI.Modules.charts;
using ExpenseTrackerApp.UI.Modules.ExpenseList;
using ExpenseTrackerApp.UI.Modules.Login;
using ExpenseTrackerApp.UI.Modules.TabbedHome;
using FreshMvvm.Maui.Extensions;
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
                .UseDevExpress()
                .UseDevExpressCharts()

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
                })
                 .UseMauiCommunityToolkit(options =>
                {
                    options.SetShouldSuppressExceptionsInBehaviors(true);
                });

            builder.UseMauiApp<App>().UseMauiCommunityToolkitCore();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginPageModel>();

            builder.Services.AddTransient<HomeTabbedPage>();
            builder.Services.AddTransient<HomeTabbedPageModel>();

            builder.Services.AddTransient<AddEditExpensePage>();
            builder.Services.AddTransient<AddEditExpensePageModel>();


            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<CategoryPageModel>();
             
            builder.Services.AddTransient<ExpenseListPage>();
            builder.Services.AddTransient<ExpenseListPageModel>();

            builder.Services.AddTransient<ChartPage>();
            builder.Services.AddTransient<ChartPageModel>();

           
            builder.Services.AddSingleton<IHttpProvider, HttpProvider>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IExpenseService, ExpenseService>();
            builder.Services.AddSingleton<ICacheService, PreferenceCacheService>();
             
             var mauiApp = builder.Build();
 
             // Initialize FreshMvvm 
            mauiApp.UseFreshMvvm();
            return mauiApp; // Return the built app
        }
    }
}
