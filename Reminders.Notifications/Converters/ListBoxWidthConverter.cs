using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Reminders.Notifications.Converters
{
    public class ListBoxWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 3)
            {
                var actualWidth = System.Convert.ToDouble(values[0]);
                var pMargin = (Thickness)values[1];
                var tbMargin = (Thickness)values[2];
                return actualWidth - pMargin.Left - pMargin.Right - tbMargin.Left - tbMargin.Right - 20;
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}