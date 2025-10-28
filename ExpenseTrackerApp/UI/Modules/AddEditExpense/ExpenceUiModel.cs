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
                CheckValid();
                RaisePropertyChanged();
            }
        }
        public Guid ? Id { get; set; }
        private string expenseDescrption;
        public string ExpenseDescrption
        {
            get => expenseDescrption;
            set
            {
                if (expenseDescrption == value) return;
                expenseDescrption = value;
                CheckValid();
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
                CheckValid();
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

        private void CheckValid()
        {
            IsValid = !string.IsNullOrWhiteSpace(ExpenseDescrption) && Amount > 0 && Category != null;  
        }

        private bool isValid;   
        public bool IsValid
        {
            get=> isValid;
            set
            {
                if (isValid == value) return;
                isValid = value;
                RaisePropertyChanged(); 
            }
        }
    }
}
