using System;
using System.Globalization;
using Xamarin.Forms;

namespace ExpenseTracker.Converters
{
    public class ExpensesConverter : IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"Out: €{value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
