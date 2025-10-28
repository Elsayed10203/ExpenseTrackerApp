
namespace ExpenseTrackerApp.Services.Auth;
public interface  IAuthService
{
    Task<bool> Login(string email,string pass);
}
