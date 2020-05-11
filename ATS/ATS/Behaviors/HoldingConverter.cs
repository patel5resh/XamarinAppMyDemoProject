using Syncfusion.ListView.XForms;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ATS.Behaviors
{
    public class HoldingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemHoldingEventArgs eventArgs = null;

            if (value is ItemHoldingEventArgs)
            {
                eventArgs = value as ItemHoldingEventArgs;
            }

            return eventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
