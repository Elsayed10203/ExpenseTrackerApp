using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.UI.Core;
using ExpenseTrackerApp.UI.FontAwesome;
using ExpenseTrackerApp.UI.Modules.AddEditExpense;
using System.Collections.ObjectModel;
 
namespace ExpenseTrackerApp.UI.Modules.Category
{
    public class CategoryPageModel : BaseCategoryPageModel
    {

        private CategoryUiModel selectedCatg;
        public CategoryPageModel()
        {
            SelectCategoryCommand = new RelayCommand<CategoryUiModel>(ExecuteSelectCategory);
            CloseCommand = new RelayCommand(async () =>
            {
                await CoreMethods.PopToRoot(false);
            });

         }

        
        #region Properties  
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing == value) return;
                _isRefreshing = value;
                RaisePropertyChanged();
            }
        }

        #endregion
        public IRelayCommand SelectCategoryCommand { get; set; }
        public IRelayCommand CloseCommand { get; set; }

         private async void ExecuteSelectCategory(CategoryUiModel catg)
        {
            try
            {
                 selectedCatg = catg;
                foreach (var item in Categories)
                {
                    item.IsSelected = (item.CatId == catg.CatId);
                }

                 await CoreMethods.PushPageModel<AddEditExpensePageModel>(new NavParams() {
                     { "categ-item", selectedCatg } });
            }
            catch{ }
             
        }       
        
    }
}
