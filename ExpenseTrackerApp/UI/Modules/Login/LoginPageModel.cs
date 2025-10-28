using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Services.Auth;
using ExpenseTrackerApp.UI.FontAwesome;
 using ExpenseTrackerApp.UI.Modules.TabbedHome;
using FreshMvvm.Maui;

namespace ExpenseTrackerApp.UI.Modules.Login
{
    public partial class LoginPageModel : BasePageModel
    {
        private readonly IAuthService authService;

        public LoginPageModel(IAuthService authService)
        {
            SignInCommand = new AsyncRelayCommand(() => ExecSignInCommand(), IsInputValid, AsyncRelayCommandOptions.None);
            PasswordClickCommand = new RelayCommand(() =>
            {
                IsPassword = !IsPassword;
                PasswordEndIcon.Glyph = IsPassword ? Glyph.Eye : Glyph.EyeSlash;
            });
           
            this.authService = authService;   
        }

        #region Properties
         private string email ;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                SignInCommand.NotifyCanExecuteChanged();    
                RaisePropertyChanged();
            }
        }

        private string password = string.Empty;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                SignInCommand.NotifyCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        private FontImageSource passwordEndIcon = new FontImageSource()
        {
            //  FontFamily = "FAFS",
            FontFamily = "FAPL",
            Glyph = Glyph.Eye,
            Size = 18,
            Color = Color.FromArgb("#82849c")
        };

        public FontImageSource PasswordEndIcon
        {
            get => passwordEndIcon;
            set
            {
                passwordEndIcon = value;
                RaisePropertyChanged();
            }
        }       

        private bool isPassword=true;
        public bool IsPassword
        {
            get => isPassword;
            set
            {
                isPassword = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands
        public IRelayCommand SignInCommand { get; private set; }
        public IRelayCommand PasswordClickCommand { get; private set; }

        #endregion

      //  private bool IsInputValid() => (!(string.IsNullOrWhiteSpace(Password) || Password.Length < 3)) && !string.IsNullOrWhiteSpace(Email);
        private bool IsInputValid()
        {
            var res=(!(string.IsNullOrWhiteSpace(Password) || Password.Length < 3)) && !string.IsNullOrWhiteSpace(Email);
            return res;
        }

        private async Task ExecSignInCommand()
        {
            IsBusy = true; 
            var res =  await  authService.Login(Email, Password); 
            IsBusy = false;
            var page = FreshPageModelResolver.ResolvePageModel<HomeTabbedPageModel>();
            Application.Current.MainPage = new FreshNavigationContainer(page);
        }

    }
}
