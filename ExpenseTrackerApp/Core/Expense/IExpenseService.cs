using ExpenseTrackerApp.UI.Modules.AddEditExpense;
using System.Collections.ObjectModel;

namespace ExpenseTrackerApp.Services.Expense
{
    public interface IExpenseService
    {
        Task<List<ExpenseModel>> GetExpensesAsync();
        Task<bool> AddExpenseAsync(ExpenseModel expense);
        Task UpdateExpenseAsync(ExpenseModel expense);
        Task DeleteExpenseAsync(Guid id);
     }
}
