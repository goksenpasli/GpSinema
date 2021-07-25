using System.Windows.Controls;
using System.Windows.Data;

namespace Sinema.View
{
    /// <summary>
    /// Interaction logic for WebFilmArama.xaml
    /// </summary>
    public partial class WebFilmArama : UserControl
    {
        public static XmlDataProvider xmlDataProvider;

        public WebFilmArama()
        {
            InitializeComponent();
            DataContext = new ViewModel.FilmAramaViewModel();
            xmlDataProvider = TryFindResource("Film") as XmlDataProvider;
        }
    }
}