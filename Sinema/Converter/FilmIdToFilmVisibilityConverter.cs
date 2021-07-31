using Sinema.Model;
using Sinema.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;

namespace Sinema
{
    public class FilmIdToFilmVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            if (value is int filmid)
            {
                IEnumerable<XElement> filmler = XElement.Load(MainWindowViewModel.xmldatapath)?.Descendants("Film");
                Film film = filmler.FirstOrDefault(z => z.Attribute("Id").Value == filmid.ToString()).DeSerialize<Film>();
                return film.FilmSaati < DateTime.Now ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}