using System;
using System.Globalization;
using Xamarin.Forms;

namespace ATS.Converters
{
    public class SelectionBoolToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? Color.FromRgb(153, 194, 255) : Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
