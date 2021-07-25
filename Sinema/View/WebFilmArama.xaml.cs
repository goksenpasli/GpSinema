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

        public static XmlDataProvider xmlDataProvider;

        public WebFilmArama()
        {
            InitializeComponent();
            DataContext = new ViewModel.FilmAramaViewModel();
            xmlDataProvider = TryFindResource("Film") as XmlDataProvider;
        }

        public string DownloadedImageAdress
        {
            get => (string)GetValue(DownloadedImageAdressProperty);
            set => SetValue(DownloadedImageAdressProperty, value);
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