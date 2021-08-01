using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sinema.View
{
    /// <summary>
    /// Interaction logic for WebFilmArama.xaml
    /// </summary>
    public partial class WebFilmArama : UserControl
    {
        public static readonly DependencyProperty DownloadedImageAdressProperty =DependencyProperty.Register("DownloadedImageAdress", typeof(string), typeof(WebFilmArama), new PropertyMetadata(null));

        public static readonly DependencyProperty FilmAdıProperty = DependencyProperty.Register("FilmAdı", typeof(string), typeof(WebFilmArama), new PropertyMetadata(null, Changed));

        private static XmlDataProvider xmlDataProvider;

        public WebFilmArama()
        {
            InitializeComponent();
            xmlDataProvider = TryFindResource("Film") as XmlDataProvider;
        }

        public string DownloadedImageAdress
        {
            get => (string)GetValue(DownloadedImageAdressProperty);
            set => SetValue(DownloadedImageAdressProperty, value);
        }

        public string FilmAdı
        {
            get => (string)GetValue(FilmAdıProperty);
            set => SetValue(FilmAdıProperty, value);
        }

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                try
                {
                    xmlDataProvider.Source = new Uri($"http://www.omdbapi.com/?t={e.NewValue as string}&apikey=c30efe8&r=xml");
                }
                catch (Exception)
                {
                }
            }
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.Source is Image ımage)
            {
                DownloadedImageAdress = ımage.Source?.ToString();
            }
        }
    }
}