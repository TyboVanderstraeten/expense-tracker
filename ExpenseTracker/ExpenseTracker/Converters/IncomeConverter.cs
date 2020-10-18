using System;
using System.Globalization;
using Xamarin.Forms;

namespace ExpenseTracker.Converters
{
    public class IncomeConverter : IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"In: €{value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
