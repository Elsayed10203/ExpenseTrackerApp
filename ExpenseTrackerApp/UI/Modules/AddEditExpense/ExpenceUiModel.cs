using ExpenseTrackerApp.UI.Core;
using ExpenseTrackerApp.UI.Modules.Category;

namespace ExpenseTrackerApp.UI.Modules.AddEditExpense
{
  public  class ExpenceUiModel :ModelBase
    {
        CategoryUiModel category;  
        public CategoryUiModel Category
        {
            get => category;
            set
            {
                if (category == value) return;
                category = value;
                RaisePropertyChanged();
            }
        }  

        private string expenseDescrption;
        public string ExpenseDescrption
        {
            get => expenseDescrption;
            set
            {
                if (expenseDescrption == value) return;
                expenseDescrption = value;
                RaisePropertyChanged();
            }
        }
        private decimal amount;
        public decimal Amount
        {
            get => amount;
            set
            {
                if (amount == value) return;
                amount = value;
                RaisePropertyChanged();
            }
        }
        private DateTime expenseDate=DateTime.Now;
        public DateTime ExpenseDate
        {
            get => expenseDate;
            set
            {
                if (expenseDate == value) return;
                expenseDate = value;
                RaisePropertyChanged();
            }
        }
    }
}
