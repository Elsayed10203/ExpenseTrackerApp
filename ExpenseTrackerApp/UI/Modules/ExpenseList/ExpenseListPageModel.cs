using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Helper;
using ExpenseTrackerApp.Services.Expense;
using ExpenseTrackerApp.UI.Core;
using ExpenseTrackerApp.UI.Modules.AddEditExpense;
using ExpenseTrackerApp.UI.Modules.Category;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace ExpenseTrackerApp.UI.Modules.ExpenseList
{
    public partial class ExpenseListPageModel : BaseCategoryPageModel
    {
        private readonly IExpenseService _expenseService;

        public ExpenseListPageModel(IExpenseService expenseService)
        {
            Title = Languages.LanguagesResources.Expenses;
            this._expenseService = expenseService;
            Expenses = new ObservableCollection<ExpenceUiModel>();

            LoadExpensesCommand = new AsyncRelayCommand(LoadExpensesAsync);
            AddExpenseCommand = new AsyncRelayCommand(GoToAddExpensePage);
            EditExpenseCommand = new AsyncRelayCommand<ExpenceUiModel>(GoToEditExpensePage);
            DeleteExpenseCommand = new AsyncRelayCommand<ExpenceUiModel>(DeleteExpenseAsync);
            SelectCategoryCommand = new AsyncRelayCommand<CategoryUiModel>(ExecuteSelectCategoryCommand);

            RefreshCommand = new AsyncRelayCommand(ExecuteRefreshCommand, AsyncRelayCommandOptions.None);
        }

        #region Properties
        public List<ExpenceUiModel> AllExpenses;

        private ObservableCollection<ExpenceUiModel> expenses;

        public ObservableCollection<ExpenceUiModel> Expenses
        {
            get => expenses;
            set
            {
                if (expenses == value) return;
                expenses = value;
                Currentstate = (Expenses?.Count > 0) ? LayoutState.Success : LayoutState.Empty;
                 RaisePropertyChanged();
            }
        }

  
        private LayoutState currentstate_ = LayoutState.None;
        public LayoutState Currentstate
        {
            get => currentstate_;
            set
            {
                if (currentstate_ == value) return;
                currentstate_ = value;
                RaisePropertyChanged();
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                RaisePropertyChanged();
            }
        }

        private string totalCount;
        public string TotalCount
        {
            get => totalCount;
            set
            {
                totalCount = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Command
        public IRelayCommand SelectCategoryCommand { get; set; }
        public ICommand LoadExpensesCommand { get; }
        public IRelayCommand AddExpenseCommand { get; }
        public IRelayCommand EditExpenseCommand { get; }
        public ICommand DeleteExpenseCommand { get; }
        public ICommand ClearFilterCommand { get; }
        public IRelayCommand RefreshCommand { get; set; }

        #endregion

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            Task.Run(async () =>
           {
                await LoadExpensesAsync();
           });
            base.ViewIsAppearing(sender, e);
        }

        private void CalSumTotal()
        {
            var sum = Expenses?.Sum(x => x.Amount) ?? 0m;
             TotalCount = sum.ToString("C2", CultureInfo.CurrentCulture);
        }

        private async Task ExecuteSelectCategoryCommand(CategoryUiModel? model)
        {
            try
            {
                if (model == null) return;

                var item = Categories?.FirstOrDefault(x => x.CatId == model.CatId);
                if (item != null)
                {
                    item.IsSelected = !item.IsSelected;
                }
                foreach (var obj in Categories)
                {
                    if (obj.CatId != item.CatId)
                        obj.IsSelected = false;
                }


                // Load all expenses
                var allExpenses = await _expenseService.GetExpensesAsync();

                // Filter expenses: if any categories selected, include only those; otherwise include all
                IEnumerable<ExpenseModel> filteredExpenses = allExpenses;
                if (item.IsSelected)
                    filteredExpenses = filteredExpenses.Where(e => e.CategoryId == item.CatId);

                // Update UI collection
                Expenses.Clear();
                foreach (var expense in filteredExpenses.OrderByDescending(e => e.Date))
                {
                    Expenses.Add(MapToUiModel(expense));
                }

                CalSumTotal();
            }
            catch (Exception ex)
            {
                ShowToaster.show($"Failed to filter expenses: {ex.Message}");
            }
        }

        private async Task ExecuteRefreshCommand()
        {
            try
            {
                IsRefreshing = true;
                await LoadExpensesAsync();
                IsRefreshing = false;

            }
            catch (Exception Excep)
            {
                IsRefreshing = false;
            }
        }

        private ExpenceUiModel MapToUiModel(ExpenseModel expense)
        {
            var categoryUiModel = Categories?.FirstOrDefault(c => c.CatId == expense.CategoryId);

            return new ExpenceUiModel
            {
                Id = expense.Id,
                Category = categoryUiModel,
                ExpenseDescrption = expense.Description,
                Amount = expense.Amount,
                ExpenseDate = expense.Date
            };
        }

        protected async Task LoadExpensesAsync()
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

                Currentstate = (Expenses?.Count > 0) ? LayoutState.Success : LayoutState.Empty;

                LoadCategories();

                CalSumTotal();
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
            await CoreMethods.PushPageModel<CategoryPageModel>();
        }

        private async Task GoToEditExpensePage(ExpenceUiModel item)
        {
            await CoreMethods.PushPageModel<AddEditExpensePageModel>(new NavParams()
            {
                     { "expence-item", item } }
            );
        }

        private async Task DeleteExpenseAsync(ExpenceUiModel expense)
        {
            if (expense == null) return;

            var confirm = await CoreMethods.DisplayAlert("Delete Expense", $"Are you sure you want to delete the expense for {expense.Amount:C} ({expense.ExpenseDescrption})?", "Yes", "No");
            if (confirm)
            {
                if (IsBusy) return;
                IsBusy = true;
                try
                {
                    if (expense.Id.HasValue)
                    {
                        await _expenseService.DeleteExpenseAsync(expense.Id.Value);
                        Expenses.Remove(expense);
                        ShowToaster.show(Languages.LanguagesResources.RemovedSuccessfully);
                    }
                    else
                    {
                        ShowToaster.show(Languages.LanguagesResources.erroroccurred);
                    }

                    CalSumTotal();
                    Currentstate = (Expenses?.Count > 0) ? LayoutState.Success : LayoutState.Empty;
                }
                catch (Exception ex)
                {
                    Currentstate = LayoutState.Error;
                    ShowToaster.show(Languages.LanguagesResources.erroroccurred);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

  
    }
}
