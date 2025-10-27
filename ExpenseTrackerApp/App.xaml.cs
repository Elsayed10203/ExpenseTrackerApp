using ExpenseTrackerApp.Helper;
using ExpenseTrackerApp.UI.Modules.Login;
using FreshMvvm.Maui;

namespace ExpenseTrackerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppHandlers.Init();
            var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            MainPage = new FreshNavigationContainer(page);
        }
    }
}
