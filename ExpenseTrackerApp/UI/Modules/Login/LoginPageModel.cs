using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Services.Auth;
using ExpenseTrackerApp.UI.FontAwesome;
using ExpenseTrackerApp.UI.Modules.Home;

namespace ExpenseTrackerApp.UI.Modules.Login
{
    public class LoginPageModel : BasePageModel
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
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                SignInCommand.NotifyCanExecuteChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                SignInCommand.NotifyCanExecuteChanged();
            }
        }

        private FontImageSource _passwordEndIcon = new FontImageSource()
        {
            //  FontFamily = "FAFS",
            FontFamily = "FAPL",
            Glyph = Glyph.Eye,
            Size = 18,
            Color = Color.FromArgb("#82849c")
        };
        public FontImageSource PasswordEndIcon
        {
            get => _passwordEndIcon;
            set
            {
                _passwordEndIcon = value;
                RaisePropertyChanged();
            }
        }

        private bool _isPassword = true;
        public bool IsPassword
        {
            get { return _isPassword; }
            set
            {
                _isPassword = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public IRelayCommand SignInCommand { get; private set; }
        public IRelayCommand PasswordClickCommand { get; private set; }

        #endregion

        private bool IsInputValid() =>(!(string.IsNullOrWhiteSpace(Password) || Password.Length< 3)) && !string.IsNullOrWhiteSpace(Email);

        private async Task ExecSignInCommand()
        {
            IsBusy = true; 
            var res =  await  authService.Login(Email, Password); 
            IsBusy = false; 
          await  CoreMethods.PushPageModel<TappedHomePageModel>(null, false, true);   
        }

    }
}
