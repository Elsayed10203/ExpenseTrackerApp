using FreshMvvm.Maui;

namespace ExpenseTrackerApp.UI.Modules
{
    public partial class BasePageModel : FreshBasePageModel
    {
 
        protected BasePageModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        ~BasePageModel()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            if (e == null) return;

            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

      
        #region Properties

        private string title = string.Empty;
        public string Title
        {
            get => title;
            set
            {
                if (title == value) return;
                title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

 
        private bool isNotConnected = false;
        public bool IsNotConnected
        {
            get => isNotConnected;
            set
            {
                if (isNotConnected == value) return;
                isNotConnected = value;
                RaisePropertyChanged();
            }
        }


        private FlowDirection pageFlowDirection;
        public FlowDirection PageFlowDirection
        {
            get => pageFlowDirection;
            set
            {
                if (pageFlowDirection == value) return;
                pageFlowDirection = value;
                RaisePropertyChanged();
            }
        }


        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value) return;
                isBusy = value;
                RaisePropertyChanged();
            }
        }


        #endregion  
    }
}