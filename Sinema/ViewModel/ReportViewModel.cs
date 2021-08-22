using Extensions;
using Sinema.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace Sinema.ViewModel
{
    public class ReportViewModel : InpcBase
    {
        private ObservableCollection<GraphControl.Chart> graphData = new();

        public ReportViewModel()
        {
            KişiSiparişListesiRaporla = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\KişiSiparişRapor.xlsx";
                if (File.Exists(ExeFolder + filepath))
                {
                    var veri = parameter as CollectionView;
                    veri.CreateReport(ExeFolder + filepath);
                }
            }, parameter => parameter is not null);

            SalonDurumuRaporla = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\SalonRapor.xlsx";
                if (File.Exists(ExeFolder + filepath))
                {
                    var veri = parameter as Salon;

                    veri.Koltuklar.Where(z => z.Müşteri.Any()).CreateReport(ExeFolder + filepath);
                }
            }, parameter => parameter is not null);

            SalonFilmTutarRaporla = new RelayCommand<object>(parameter =>
            {
                const string filepath = @"\Raporlar\SalonFilmlerTutar.xlsx";
                if (File.Exists(ExeFolder + filepath))
                {
                    FilmTutarlarınıAl().CreateReport(ExeFolder + filepath);
                }
            }, parameter => true);

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

            if (File.Exists(MainWindowViewModel.xmldatapath))
            {
                var stringtobrushconverter = new StringToBrushConverter();
                foreach (var item in FilmTutarlarınıAl())
                {
                    var rand = new Random(Guid.NewGuid().GetHashCode());
                    GraphData.Add(new GraphControl.Chart() { Description = item.Adı, ChartValue = item.Fiyat, ChartBrush = (Brush)stringtobrushconverter.Convert(item.Renk, null, null, CultureInfo.CurrentUICulture) });
                }
            }
        }

        public ICommand FilmListesiRaporla { get; }

        public ObservableCollection<GraphControl.Chart> GraphData
        {
            get => graphData;

            set
            {
                if (graphData != value)
                {
                    graphData = value;
                    OnPropertyChanged(nameof(GraphData));
                }
            }
        }

        public ICommand KişiSiparişListesiRaporla { get; }

        public ICommand SalonDurumuRaporla { get; }

        public ICommand SalonFilmTutarRaporla { get; }

        private string ExeFolder { get; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        private static IEnumerable<ReportData> FilmTutarlarınıAl()
        {
            var document = XDocument.Load(MainWindowViewModel.xmldatapath);
            return document.Descendants("Müşteri").Select(z => new ReportData()
            {
                Adı = document.Descendants("Film").FirstOrDefault(d => (int)d.Attribute("Id") == (int)z.Attribute("FilmId"))?.Attribute("Adı").Value,
                FilmSaati = (DateTime)document.Descendants("Film").FirstOrDefault(d => (int)d.Attribute("Id") == (int)z.Attribute("FilmId"))?.Attribute("FilmSaati"),
                Fiyat = (double)z.Attribute("BiletFiyat"),
                Renk = (string)document.Descendants("Film").FirstOrDefault(d => (int)d.Attribute("Id") == (int)z.Attribute("FilmId"))?.Attribute("Renk")
            });
        }
    }

    internal class ReportData : Film
    {
        public double Fiyat { get; set; }
    }
}