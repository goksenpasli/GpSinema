using Sinema.ViewModel;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sinema
{
    public class KoltukNoMergeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return DesignerProperties.GetIsInDesignMode(new DependencyObject())
                ? null
                : values[0] is int koltukno && values[1] is int bölen ? ((koltukno - 1) % bölen).HarfeDöndür() + koltukno : 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}