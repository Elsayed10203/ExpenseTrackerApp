using ExpenseTrackerApp.Helper;
using ExpenseTrackerApp.UI.Modules.AddEditExpense;
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
         // var page = FreshPageModelResolver.ResolvePageModel<AddEditExpensePageModel>();
            MainPage = new FreshNavigationContainer(page);                  
        }
    }
}
