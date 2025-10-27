using CommunityToolkit.Maui.Views;

namespace ExpenseTrackerApp.UI
{
    public class SharedPopup : Popup
    {
        public SharedPopup()
        {
            Opened += (sender, e) =>
            {
                isOpened = true;
            };

            //Dismissed += (object sender, PopupDismissedEventArgs e) =>
            //{
            //    isOpened = false;
            //};
        }

        private bool isOpened = false;

        public void Show(Page page)
        {
            if (isOpened)
                return;

            page.ShowPopup(this);
        }

        public void Hide()
        {
            // Dismiss(null);
        }
    }
}
