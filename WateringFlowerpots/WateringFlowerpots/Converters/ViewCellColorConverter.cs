using System;
using System.Globalization;
using Xamarin.Forms;

namespace WateringFlowerpots.Converters
{
    public class ViewCellColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string obj)
            {
                return obj == DateTime.Today.DayOfWeek.ToString() ? ConstColors.SuperLightYellow : ConstColors.ViewCellColor;
            }
            return ConstColors.ViewCellColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
