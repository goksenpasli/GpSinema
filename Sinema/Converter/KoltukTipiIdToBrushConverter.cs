using Sinema.ViewModel;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;

namespace Sinema
{
    public class KoltukTipiIdToBrushConverter : IValueConverter
    {
        private readonly BrushConverter brushconverter;

        public KoltukTipiIdToBrushConverter()
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                brushconverter = new BrushConverter();
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            if (value is int Id)
            {
                var koltukrenkleri = XElement.Load(MainWindowViewModel.xmldatapath)?.Element("KoltukTipleri")?.Elements("KoltukTipi");
                var renk = koltukrenkleri?.Where(z => (int)z.Attribute("Id") == Id).Select(z => z.Attribute("KoltukRenk").Value).FirstOrDefault();
                return !string.IsNullOrEmpty(renk) ? brushconverter.ConvertFromString(renk) : Brushes.Transparent;
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}