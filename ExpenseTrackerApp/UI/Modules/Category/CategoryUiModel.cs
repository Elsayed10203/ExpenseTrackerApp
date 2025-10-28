
using ExpenseTrackerApp.UI.Core;

namespace ExpenseTrackerApp.UI.Modules.Category
{
    public partial class CategoryUiModel : ModelBase
    {
        public int CatId { get; set; }
        public string Name { get; set; }

         private string icon;

        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                RaisePropertyChanged();
            }
        }
         
        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                RaisePropertyChanged();
            }
        }

     //   public FontImageSource ImageSrc => new() { FontFamily = "FAPL", Glyph = Icon, Size = 25, Color = Colors.Gray };

        private string backColor;
        public string BackColor
        {
            get => backColor;
            set
            {
                backColor = value;
                RaisePropertyChanged();
            }
        }
     }
}
