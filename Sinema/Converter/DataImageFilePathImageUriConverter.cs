using Sinema.ViewModel;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Sinema
{
    public class DataImageFilePathImageUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()) && value is string filename && !string.IsNullOrEmpty(filename) && File.Exists($"{Path.GetDirectoryName(MainWindowViewModel.xmldatapath)}\\{filename}"))
            {
                BitmapImage image = new();
                image.BeginInit();
                image.DecodePixelHeight = int.TryParse((string)parameter, out var res) ? res : 96;
                image.CacheOption = BitmapCacheOption.None;//onload to bypass file lock
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri($"{Path.GetDirectoryName(MainWindowViewModel.xmldatapath)}\\{filename}");
                image.EndInit();
                if (!image.IsFrozen && image.CanFreeze)
                {
                    image.Freeze();
                }
                return image;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}