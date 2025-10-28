using ExpenseTrackerApp.UI.FontAwesome;
using System.Collections.ObjectModel;

namespace ExpenseTrackerApp.UI.Modules.Category
{
    public  class BaseCategoryPageModel : BasePageModel
    {

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

        public BaseCategoryPageModel()
        {
            Categories =  new ObservableCollection<CategoryUiModel>(GetDefaultCategories()); 
        }

        protected void LoadCategories()
        {
            Categories = new ObservableCollection<CategoryUiModel>(GetDefaultCategories());
        }

        protected List<CategoryUiModel> GetDefaultCategories()
        {
            return new List<CategoryUiModel>
            {
                new CategoryUiModel {CatId=1, Name = "Food", Icon = Glyph.Utensils, BackColor = "#FFC0CB" }, // Pink
                new CategoryUiModel {CatId=2, Name = "Transport", Icon = Glyph.Car, BackColor = "#ADD8E6" }, // Light Blue
                new CategoryUiModel {CatId=3, Name = "Entertainment", Icon = Glyph.Popcorn, BackColor = "#90EE90" }, // Light Green
                new CategoryUiModel {CatId=4, Name = "Shopping", Icon = Glyph.CartShopping, BackColor = "#FFD700" }, // Gold
                new CategoryUiModel {CatId=5, Name = "Bills", Icon = Glyph.Receipt, BackColor = "#D3D3D3" }, // Light Gray
                new CategoryUiModel {CatId=6, Name = "Health", Icon = Glyph.HeartPulse, BackColor = "#FF6347" }, // Tomato Red
                new CategoryUiModel {CatId=7, Name = "Travel", Icon = Glyph.Plane, BackColor = "#87CEEB" }, // Sky Blue
                new CategoryUiModel {CatId=8, Name = "Education", Icon = Glyph.GraduationCap, BackColor = "#BA55D3" }, // Medium Orchid (Added a common category)
                new CategoryUiModel {CatId=9, Name = "Home", Icon = Glyph.House, BackColor = "#F4A460" }, // Sandy Brown (Added a common category)
                new CategoryUiModel {CatId=10, Name = "Salary", Icon = Glyph.Wallet, BackColor = "#32CD32" }, // Lime Green (Example for income, if tracking)
                new CategoryUiModel {CatId=11, Name = "Others", Icon = Glyph.SquareQuestion, BackColor = "#A9A9A9" } // Dark Gray
            };
        }


    }
}
