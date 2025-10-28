using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace ExpenseTrackerApp.Converters
{
    public class LayoutStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LayoutState param = (LayoutState)parameter;
            var check = value?.Equals(param);
            return check;
            return value?.Equals(param);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
