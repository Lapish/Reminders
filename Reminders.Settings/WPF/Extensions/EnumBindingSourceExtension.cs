using System;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

namespace Reminders.Settings.WPF.Extensions
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; set; }

        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType is null)
            {
                throw new Exception("Type must not be null");
            }

            if (!enumType.IsEnum)
            {
                throw new Exception("Type must be an enum");
            }

            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            string[] names = Enum.GetNames(EnumType);
            string[] values = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                object localizeName = Application.Current.Resources[$"Enum.{EnumType.Name}.{names[i]}"];
                if (localizeName is null)
                {
                    values[i] = "";
                }
                else
                {
                    values[i] = (string)localizeName;
                }
            }

            return values;
        }
    }
}
