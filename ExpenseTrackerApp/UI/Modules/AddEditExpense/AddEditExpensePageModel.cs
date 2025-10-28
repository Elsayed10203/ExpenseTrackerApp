using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Helper;
using ExpenseTrackerApp.Services.Expense;
using ExpenseTrackerApp.UI.Core;
using ExpenseTrackerApp.UI.Modules.Category;

namespace ExpenseTrackerApp.UI.Modules.AddEditExpense
{
  public class AddEditExpensePageModel : BasePageModel
    {
        IExpenseService expenseService;
        public AddEditExpensePageModel(IExpenseService expenseService)
        {
            this.expenseService = expenseService;   
            AddCommand = new AsyncRelayCommand(()=>ExecuteAddCommand(), canExecute: () => true, AsyncRelayCommandOptions.None);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
         }

        #region Properties 

        private ExpenceUiModel expenceItem=new ExpenceUiModel();
        public ExpenceUiModel ExpenceItem
        {
            get => expenceItem;
            set
            {
                expenceItem = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public IRelayCommand AddCommand { get; set; }
        public IRelayCommand CancelCommand { get; set; }
        #endregion
        private async void ExecuteCancelCommand()
        {
             await CoreMethods.PopPageModel();  
        }

        private async Task ExecuteAddCommand()
        {
            IsBusy = true;
            var expenseDTO = MapUiModelToExpence(ExpenceItem);  // Map UI model to DTO   

            var res= await expenseService?.AddExpenseAsync(expenseDTO);
            IsBusy = false;

            ShowToaster.show("added successfully....");  

            await CoreMethods.PopToRoot(false);   
        }

        private  ExpenseModel MapUiModelToExpence(ExpenceUiModel expenceUiModel)
        {
            if (expenceUiModel == null)
                throw new ArgumentNullException(nameof(expenceUiModel));

            return new ExpenseModel
            {
                Id = Guid.NewGuid(),
                Amount = expenceUiModel.Amount,
                CategoryId = expenceUiModel.Category != null ? expenceUiModel.Category.CatId : 0,
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

                if (navParams.ContainsKey("categ-item") && navParams["categ-item"] is CategoryUiModel ItemModel)
                {
                   if(ExpenceItem != null)
                        ExpenceItem.Category = (ItemModel as CategoryUiModel);
                }
            }
            catch (Exception)
            {

               
            }
            base.Init(initData);
        }
    }
}
