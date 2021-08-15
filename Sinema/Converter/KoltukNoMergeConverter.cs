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
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            var data = values;
            return data[0] is int koltukno && data[1] is int bölen ? ExtensionMethods.HarfeDöndür((koltukno - 1) % bölen) + koltukno : 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}