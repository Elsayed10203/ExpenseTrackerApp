using ExpenseTrackerApp.UI.Core;

namespace ExpenseTrackerApp.UI.Modules.charts
{
    public class ChartUiModel: ModelBase
    {

        private int catgId;
        public int CatgId 
        { get=> catgId;
            set
            {
                catgId = value;
                RaisePropertyChanged();
            }
        }

        private string catgName;    
        public string CatgName
        {
            get=> catgName;
            set
            {
                catgName = value;
                RaisePropertyChanged();
            }
        }

        private double amount;
        public double Amount 
        { 
            get=> amount;
            set
            {
                amount = value;
                RaisePropertyChanged();
            }
        }
    }
}
