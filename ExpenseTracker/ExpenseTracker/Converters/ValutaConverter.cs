using System;
using System.Globalization;
using Xamarin.Forms;

namespace ExpenseTracker.Converters
{
    public class ValutaConverter : IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal castedValue = (decimal)value;
            if (castedValue < 0)
            {
                return $"- €{Math.Abs(castedValue)}";
            }
            else
            {
                return $"€{castedValue}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
