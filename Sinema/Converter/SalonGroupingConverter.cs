using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Sinema
{
    public class SalonGroupingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is IGrouping<int, int> group ? group.ElementAt(0) + "X" + group.ElementAt(1) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}