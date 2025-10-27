using ExpenseTrackerApp.Services.IHttpClient;
using Newtonsoft.Json;
using System.Text;

namespace ExpenseTrackerApp.Services.Auth
{
    public class AuthService : IAuthService
    {
        IHttpProvider httpProvider;
        public AuthService(IHttpProvider httpProvider)
        {
            this.httpProvider = httpProvider;
        }
        
        public async Task<bool> Login(string email, string pass)
        {
            try
            {
                await Task.Delay(3000);
                var jContent = JsonConvert.SerializeObject(new { Email = email, Password = pass });
                var result = await httpProvider.PostAsync("Api/Login", new StringContent(jContent, Encoding.UTF8, "application/json"));
                result.EnsureSuccessStatusCode();
               
                //Save Token
            }
            catch (Exception Excep)
            {
                //Handle Error
            }

            return true;
        }
    }
}
