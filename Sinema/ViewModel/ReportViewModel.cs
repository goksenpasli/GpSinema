using Extensions;
using Sinema.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace Sinema.ViewModel
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            KişiSiparişListesiRaporla = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\KişiSiparişRapor.xlsx";
                if (File.Exists(ExeFolder + filepath))
                {
                    CollectionView veri = parameter as CollectionView;
                    veri.CreateReport(ExeFolder + filepath);
                }
            }, parameter => parameter is not null);

            SalonDurumuRaporla = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\SalonRapor.xlsx";
                if (File.Exists(ExeFolder + filepath))
                {
                    Salon veri = parameter as Salon;

                    veri.Koltuklar.Where(z => z.Müşteri.Any()).CreateReport(ExeFolder + filepath);
                }
            }, parameter => parameter is not null);

            FilmListesiRaporla = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\Filmler.xlsx";
                if (File.Exists(ExeFolder + filepath))
                {
                    List<Salon> list = new();
                    (XElement.Load(MainWindowViewModel.xmldatapath)?.Descendants("Salon")).ToList().ForEach(z => list.Add(z.DeSerialize<Salon>()));
                    list.CreateReport(ExeFolder + filepath);
                    list = null;
                }
            }, parameter => true);
        }

        public ICommand FilmListesiRaporla { get; }

        public ICommand KişiSiparişListesiRaporla { get; }

        public ICommand SalonDurumuRaporla { get; }

        private string ExeFolder { get; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    }
}