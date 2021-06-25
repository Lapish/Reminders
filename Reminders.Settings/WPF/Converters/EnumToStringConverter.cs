using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Reminders.Settings.WPF.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object s = Application.Current.Resources[$"Enum.{value.GetType().Name}.{value}"];
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (string enumValue in Enum.GetNames(targetType))
            {
                string s = $"Enum.{targetType.Name}.{enumValue}";
                object localizeName = Application.Current.Resources[s];

                if ((string)value == (string)localizeName)
                {
                    return enumValue;
                }
            }
            throw new Exception($"ConvertBack cannot convert value {value}");
        }
    }
}