using Extensions;
using System.Configuration;
using System.IO;

namespace Sinema.ViewModel
{
    public class MainWindowViewModel : InpcBase
    {
        public static readonly string xmldatapath = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath) + @"\Data.xml";

        public MainWindowViewModel()
        {
            SalonViewModel = new SalonViewModel();
            FilmViewModel = new FilmViewModel();
            KişiGirişiViewModel = new KişiGirişiViewModel();
            ÜrünGirişiViewModel = new ÜrünGirişiViewModel();
            KoltukTipiGirişiViewModel = new KoltukTipiGirişiViewModel();
            ReportViewModel = new ReportViewModel();
        }

        public FilmViewModel FilmViewModel { get; set; }

        public KişiGirişiViewModel KişiGirişiViewModel { get; set; }

        public KoltukTipiGirişiViewModel KoltukTipiGirişiViewModel { get; set; }

        public ReportViewModel ReportViewModel { get; set; }

        public SalonViewModel SalonViewModel { get; set; }

        public ÜrünGirişiViewModel ÜrünGirişiViewModel { get; set; }
    }
}