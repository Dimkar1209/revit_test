using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RevitConduitTable.WPF.Extensions
{
    public class DictionaryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Your existing logic here...

            // Validation logic
            if (IsValid(value))
            {
                return Brushes.Green; // Valid value color
            }
            else
            {
                return Brushes.Red; // Invalid value color
            }
        }

        private bool IsValid(object value)
        {
            // Implement your validation logic here
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
