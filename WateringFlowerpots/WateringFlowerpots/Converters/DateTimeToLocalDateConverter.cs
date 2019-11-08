using System;
using System.Globalization;
using Xamarin.Forms;

namespace WateringFlowerpots.Converters
{
    public class DateTimeToLocalDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("dd.MM.yy");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now.ToString("dd.MM.yy");
        }
    }
}
