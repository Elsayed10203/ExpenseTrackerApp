using Android.App;
using Android.Runtime;

namespace ExpenseTrackerApp
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public override void OnCreate()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            base.OnCreate();
        }
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            try
            {
                Exception e = (Exception)args.ExceptionObject;
                Console.WriteLine("MyHandler caught : " + e.Message);
                Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            }
            catch { }
        }
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
