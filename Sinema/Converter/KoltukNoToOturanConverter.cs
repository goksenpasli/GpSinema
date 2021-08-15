using Sinema.Model;
using Sinema.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;

namespace Sinema
{
    public class KoltukNoToOturanConverter : IMultiValueConverter
    {
        private ObservableCollection<Musteri> musteriler;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }

            musteriler = new ObservableCollection<Musteri>();

            if (values[0] is int koltukno && values[1] is int salonensayısı)
            {
                foreach (var item in XDocument.Load(MainWindowViewModel.xmldatapath)?.Descendants("Koltuk").Where(z => (int)z.Attribute("No") >= ((koltukno - 1) * salonensayısı) + 1 && (int)z.Attribute("No") <= koltukno * salonensayısı).Descendants("Müşteri"))
                {
                    musteriler.Add(item.DeSerialize<Musteri>());
                }
                return musteriler;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}