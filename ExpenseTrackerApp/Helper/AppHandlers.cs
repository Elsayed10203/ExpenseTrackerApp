#if ANDROID
using Android.App;
using Android.Content;
using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Platform;
#endif
#if IOS
using UIKit;
#endif

namespace ExpenseTrackerApp.Helper
{
    internal class AppHandlers
    {
        public static void Init()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {

#if ANDROID
                handler.PlatformView.SetPadding(0, 0, 0, 0);
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.SetSelectAllOnFocus(true);
#elif IOS
         handler.PlatformView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
          //Added
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
            handler.PlatformView.Layer.BorderWidth = 0; // Remove the border
            handler.PlatformView.Layer.BackgroundColor = UIKit.UIColor.Clear.CGColor; // Set transparent background
#endif
            });




            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });

            //EditorHandler
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);

                handler.PlatformView.SetPadding(0, 0, 0, 0);
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.SetSelectAllOnFocus(true);
#endif
            });
   
           
         }
    }
}