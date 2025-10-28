using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Helper;
using ExpenseTrackerApp.Services.Expense;
using ExpenseTrackerApp.UI.Core;
using ExpenseTrackerApp.UI.Modules.Category;

namespace ExpenseTrackerApp.UI.Modules.AddEditExpense
{
    public class AddEditExpensePageModel : BaseCategoryPageModel
    {
        IExpenseService expenseService;
        public AddEditExpensePageModel(IExpenseService expenseService)
        {
            this.expenseService = expenseService;
            AddEditCommand = new AsyncRelayCommand(() => ExecuteAddCommand(), canExecute: () => true, AsyncRelayCommandOptions.None);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            CategoryTapeCommand = new RelayCommand<object>(ExecuteCategoryTapeCommand);
        }

        #region Properties 

        private ExpenceUiModel expenceItem = new ExpenceUiModel();
        public ExpenceUiModel ExpenceItem
        {
            get => expenceItem;
            set
            {
                expenceItem = value;
                RaisePropertyChanged();
            }
        }

        private bool isEditMode = false;
        public bool IsEditMode
        {
            get => isEditMode;
            set
            {
                isEditMode = value;
                RaisePropertyChanged();
            }
        }


        #endregion

        #region Commands
        public IRelayCommand AddEditCommand { get; set; }
        public IRelayCommand CancelCommand { get; set; }
        public IRelayCommand CategoryTapeCommand { get; set; }
        #endregion


        private void ExecuteCategoryTapeCommand(object item)
        {
            if (!IsEditMode)
            {
                CancelCommand.Execute(null);
            }
            else
            {
                if (item is Picker pcker)
                    pcker.Focus();
            }
        }
        private async void ExecuteCancelCommand()
        {
            await CoreMethods.PopPageModel();
        }

        private async Task ExecuteAddCommand()
        {
            IsBusy = true;
            var isUpdate = ExpenceItem.Id.HasValue;

            var expenseDTO = MapUiModelToExpence(ExpenceItem);  // Map UI model to DTO   
            if (!isUpdate)
                await expenseService?.AddExpenseAsync(expenseDTO);
            else
                await expenseService?.UpdateExpenseAsync(expenseDTO);
            IsBusy = false;

            ShowToaster.show(Languages.LanguagesResources.addedSuccessfully);

            await CoreMethods.PopToRoot(false);
        }

        private ExpenseModel MapUiModelToExpence(ExpenceUiModel expenceUiModel)
        {
            return new ExpenseModel
            {
                Id = expenceUiModel.Id ?? Guid.NewGuid(),
                Amount = expenceUiModel.Amount,
                CategoryId = expenceUiModel.Category?.CatId ?? 0,
                Date = expenceUiModel.ExpenseDate,
                Description = expenceUiModel.ExpenseDescrption
            };
        }


        public override void Init(object initData)
        {
            try
            {
                if (initData == null || !(initData is NavParams navParams) || navParams.Count < 1)
                    return;
                // ExpenceUiModel = new ExpenceUiModel();

                if (navParams.ContainsKey("categ-item") && navParams["categ-item"] is CategoryUiModel itemCateg)
                {
                    if (ExpenceItem != null)
                        ExpenceItem.Category = (itemCateg as CategoryUiModel);
                    Categories.FirstOrDefault(c => c.CatId == itemCateg.CatId).IsSelected = true;
                    IsEditMode = false;
                    this.Title = $"{Languages.LanguagesResources.add} {Languages.LanguagesResources.Expenses}";
                }


                if (navParams.ContainsKey("expence-item") && navParams["expence-item"] is ExpenceUiModel itemExpen)
                {
                    if (ExpenceItem != null)
                        ExpenceItem = (itemExpen as ExpenceUiModel);
                    IsEditMode = true;
                    this.Title = $"{Languages.LanguagesResources.Edit} {Languages.LanguagesResources.Expenses}";

                }

            }
            catch (Exception)
            {


            }
            base.Init(initData);
        }
    }
}
