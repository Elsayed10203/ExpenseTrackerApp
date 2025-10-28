using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Helper;
using ExpenseTrackerApp.Services.Expense;
using ExpenseTrackerApp.UI.Modules.AddEditExpense;
using ExpenseTrackerApp.UI.Modules.Category;
using Java.Sql;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ExpenseTrackerApp.UI.Modules.ExpenseList
{
    public partial class ExpenseListPageModel : BasePageModel
    {
        private readonly IExpenseService _expenseService;
       
        public ExpenseListPageModel(IExpenseService expenseService)
        {
            Title = Languages.LanguagesResources.Expenses;
            this._expenseService = expenseService;
            Expenses = new ObservableCollection<ExpenceUiModel>();
  
            LoadExpensesCommand = new AsyncRelayCommand(LoadExpensesAsync);
            AddExpenseCommand = new AsyncRelayCommand(GoToAddExpensePage);
            EditExpenseCommand = new AsyncRelayCommand(GoToEditExpensePage);
            DeleteExpenseCommand = new AsyncRelayCommand<ExpenseModel>(DeleteExpenseAsync);
            ApplyFilterCommand = new AsyncRelayCommand(ApplyFilterAsync);
            ClearFilterCommand = new AsyncRelayCommand(ClearFilterAsync);
        }

        #region Properties

        private ObservableCollection<ExpenceUiModel> expenses;

        public  ObservableCollection<ExpenceUiModel> Expenses
        {
            get => expenses;
            set
            {
                if (expenses == value) return;
                expenses = value;
               RaisePropertyChanged();
            }
        }

         bool IsEmptyStateVisible;

         string SelectedCategoryFilter;

         DateTime? StartDateFilter;

         DateTime? EndDateFilter;
         
        private string title = string.Empty;

        private ObservableCollection<CategoryUiModel> categories;   
        public ObservableCollection<CategoryUiModel> Categories
        {
            get => categories;
            set
            {
                if (categories == value) return;
                categories = value;
                RaisePropertyChanged();
            }
        }

        private LayoutState currentstate= LayoutState.None;
        public LayoutState Currentstate
        {
            get => currentstate;
            set
            {
                if (currentstate == value) return;
                currentstate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Command
        public ICommand LoadExpensesCommand { get; }
        public IRelayCommand AddExpenseCommand { get; }
        public IRelayCommand EditExpenseCommand { get; }
        public ICommand DeleteExpenseCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ClearFilterCommand { get; }
        #endregion

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            Categories=CategoryPageModel.GetDefaultCategories().ToObservableCollection();
            Task.Run(async () =>
            {
                await LoadExpensesAsync();
            });
            base.ViewIsAppearing(sender, e);
        }

        private ExpenceUiModel MapToUiModel(ExpenseModel expense)
        {
             var categoryUiModel = Categories?.FirstOrDefault(c => c.CatId == expense.CategoryId);

            return new ExpenceUiModel
            {
                Category = categoryUiModel,
                ExpenseDescrption = expense.Description,
                Amount = expense.Amount,
                ExpenseDate = expense.Date
            };
        }
         
        private async Task LoadExpensesAsync()
        {
            Currentstate = LayoutState.Loading;  
 
            IsBusy = true;
            try
            {
                var allExpenses = await _expenseService.GetExpensesAsync();
                Expenses.Clear();
                foreach (var expense in allExpenses)
                {
                    Expenses.Add(MapToUiModel(expense));
                }

                Currentstate =(Expenses?.Count>0)? LayoutState.Success: LayoutState.Empty;
            }
            catch (Exception ex)
            {
                Currentstate = LayoutState.Error;
                ShowToaster.show($"Failed to load expenses: {ex.Message}");
             }
            finally
            {
                IsBusy = false;
            }
        }
 
        private async Task GoToAddExpensePage()
        {
          await  CoreMethods.PushPageModel<CategoryPageModel>();       
        }

        private async Task GoToEditExpensePage( )
        {

          }

        private async Task DeleteExpenseAsync(ExpenseModel expense)
        {
            if (expense == null) return;

            var confirm = await CoreMethods.DisplayAlert("Delete Expense", $"Are you sure you want to delete the expense for {expense.Amount:C} ({expense.Description})?", "Yes", "No");
            if (confirm)
            {
                if (IsBusy) return;
                IsBusy = true;
                try
                {
                    await _expenseService.DeleteExpenseAsync(expense.Id);
                    Expenses.Remove(MapToUiModel(expense));

                }
                catch (Exception ex)
                {
                    await CoreMethods.DisplayAlert("Error", $"Failed to delete expense: {ex.Message}", "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task ApplyFilterAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var allExpenses = await _expenseService.GetExpensesAsync();
                var filteredExpenses = allExpenses.AsEnumerable();

              //  if (SelectedCategoryFilter.Contains != "All")
                {
                //    filteredExpenses = filteredExpenses.Where(e => e.CategoryId == SelectedCategoryFilter);
                }

                if (StartDateFilter.HasValue)
                {
                    filteredExpenses = filteredExpenses.Where(e => e.Date.Date >= StartDateFilter.Value.Date);
                }

                if (EndDateFilter.HasValue)
                {
                    filteredExpenses = filteredExpenses.Where(e => e.Date.Date <= EndDateFilter.Value.Date);
                }

                Expenses.Clear();
                foreach (var expense in filteredExpenses.OrderByDescending(e => e.Date))
                {
                    Expenses.Add(MapToUiModel(expense));
                }

            }
            catch (Exception ex)
            {
                 await CoreMethods.DisplayAlert("Error", $"Failed to apply filter: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ClearFilterAsync()
        {
            SelectedCategoryFilter = "All";
            StartDateFilter = null;
            EndDateFilter = null;
            await LoadExpensesAsync();
        }
    }
}
