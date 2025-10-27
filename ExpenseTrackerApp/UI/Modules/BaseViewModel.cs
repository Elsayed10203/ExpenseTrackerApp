using FreshMvvm.Maui;

namespace ExpenseTrackerApp.UI.Modules
{
    public   class BasePageModel : FreshBasePageModel
    {
        protected BasePageModel()
        {
            FlowDirection = IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        ~BasePageModel()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
         }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;

            try
            {
               // if (IsNotConnected)
                   // DisconnectionPopup.Instance.Show(CurrentPage);
              //  else
                //    CurrentPage.HideCpPopup(DisconnectionPopup.Instance);
            }
            catch { }
        }


        #region Properties

        private string title;
        public string Title
        {
            get => title;
            protected
                set
            {
                title = value;
                RaisePropertyChanged();
            }
        }

        public bool IsViewModelLoaded { get; private set; } = false;

        public Func<bool> IsLoadedFunc => () => IsViewModelLoaded;

        private bool isNotConnected = false;
        public bool IsNotConnected
        {
            get => isNotConnected;

            private set
            {
                isNotConnected = value;
                RaisePropertyChanged();
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            protected set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        private FlowDirection flowDirection;
        public FlowDirection FlowDirection
        {
            get => flowDirection;
            protected set
            {
                flowDirection = value;
                RaisePropertyChanged();
            }
        }

        private bool isRtl;
        public bool IsRtl
        {
            get => isRtl;
            protected set
            {
                isRtl = value;
                RaisePropertyChanged();
            }
        }

        private string currentLang;
        public string CurrentLang
        {
            get => currentLang;
            protected set
            {
                currentLang = value;
                RaisePropertyChanged();
            }
        }
        #endregion


    }
}
