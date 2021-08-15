using Sinema.Model;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Sinema
{
    public class SeçiliFilmtoKoltukRenkConverter : IMultiValueConverter
    {
        private readonly StringToBrushConverter StringToBrushConverter;

        public SeçiliFilmtoKoltukRenkConverter()
        {
            StringToBrushConverter = new StringToBrushConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            return values[0] is Koltuk koltuk && values[1] is Film film && koltuk.Müşteri.Any(z => z.FilmId == film.Id)
                ? StringToBrushConverter.Convert(film.Renk, null, null, CultureInfo.CurrentCulture)
                : Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}