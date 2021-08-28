using Sinema.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Sinema
{
    public class TopluGirişKoltukConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values[0] is Koltuk koltuk && values[1] is Film film && koltuk.KoltukEtkin && koltuk.Görünür && !koltuk.Müşteri.Any(z => z.FilmId == film.Id);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}