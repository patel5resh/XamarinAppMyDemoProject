using System;
using System.Globalization;
using Xamarin.Forms;

namespace ATS.Converters
{
    public class SelectionChangedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object eventArgs = null;
            if (value is Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs)
                eventArgs = value as Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs;
            return eventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
