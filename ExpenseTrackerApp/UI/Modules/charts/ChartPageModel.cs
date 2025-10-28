using ExpenseTrackerApp.Services.Expense;
using ExpenseTrackerApp.UI.Modules.ExpenseList;

namespace ExpenseTrackerApp.UI.Modules.charts
{
   public class ChartPageModel:ExpenseListPageModel
    {
        public ChartPageModel(IExpenseService expenseService) : base(expenseService) { }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            LoadExpensesCommand?.Execute(default);

            base.ViewIsAppearing(sender, e);
        }
         
    }
}
