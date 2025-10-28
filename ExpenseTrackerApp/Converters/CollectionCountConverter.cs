using System.Globalization;

namespace ExpenseTrackerApp.Converters
{
    /// <summary>
     /// </summary>
    public class CollectionCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
                return !(count > 0);
 
            return true; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;    
        }
    }
}

