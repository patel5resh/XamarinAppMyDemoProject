using Syncfusion.ListView.XForms;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ATS.Behaviors
{
    public class CustomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemSelectionChangedEventArgs eventArgs = null;

            if (value is ItemSelectionChangedEventArgs)
            {
                eventArgs = value as ItemSelectionChangedEventArgs;
            }

            return eventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
