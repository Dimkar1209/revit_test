using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace RevitConduitTable.WPF.Extensions
{
    public class DictionaryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IDictionary<string, object> dictionary && parameter is string key)
            {
                dictionary.TryGetValue(key, out var result);
                return result;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
