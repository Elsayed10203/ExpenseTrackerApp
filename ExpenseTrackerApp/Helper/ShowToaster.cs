 using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core; // Add this using directive

namespace ExpenseTrackerApp.Helper
{
    public static class ShowToaster
    {
        public async static void  show(string messagetext)
        {
            try
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                ToastDuration duration = ToastDuration.Short; // ToastDuration is defined in CommunityToolkit.Maui.Core
                double fontSize = 14;

 
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var toast = Toast.Make(messagetext, duration, fontSize);
                    await toast.Show(cancellationTokenSource.Token);
                });
             }
            catch (Exception excep) 
            {
            
            }
			 
        }
    }
}
