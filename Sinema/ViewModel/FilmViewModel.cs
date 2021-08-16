using Extensions;
using Sinema.Model;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Sinema.ViewModel
{
    public class FilmViewModel : InpcBase
    {
        private static readonly CollectionViewSource cvsfilmler = (CollectionViewSource)Application.Current?.MainWindow?.TryFindResource("Filmler");

        private Film film;

        private string filmAramaMetni;

        private string filmTipi;

        private string saat = "00:00";

        private Salon seçiliSalon;

        private DateTime tarih = DateTime.Today;

        private bool tarihiGeçenFilmleriGizle = true;

        public FilmViewModel()
        {
            Film = new Film();

            FilmGirişiYap = new RelayCommand<object>(parameter =>
            {
                if (!SeçiliSalon.Film.Any(z => z.FilmSaati == FilmSaatiniAl()))
                {
                    Film film = new()
                    {
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        Adı = Film.Adı,
                        Süre = Film.Süre,
                        FilmTipi = FilmTipi,
                        VideoYolu = Film.VideoYolu,
                        ResimYolu = Film.ResimYolu,
                        Oyuncular = Film.Oyuncular,
                        Yönetmen = Film.Yönetmen,
                        Renk = Film.Renk,
                        FilmSaati = FilmSaatiniAl()
                    };

                    SeçiliSalon.Film.Add(film);
                    (parameter as Salonlar).Serialize();
                    Film.ResimYolu = null;
                    Film.VideoYolu = null;
                    Film.Adı = null;
                }
                else
                {
                    MessageBox.Show("Bu Saatte Bu Salon İçin Zaten Film Ayarlanmış Tarihi Değiştirin.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }, parameter => SeçiliSalon is not null && !string.IsNullOrWhiteSpace(Film?.Adı) && DateTime.TryParseExact(Saat, "H:m", new CultureInfo("tr-TR"), DateTimeStyles.None, out _));

            FilmVideoEkle = new RelayCommand<object>(parameter => ExtensionMethods.VideoEkle(Film), parameter => true);

            FilmResimEkle = new RelayCommand<object>(parameter => ExtensionMethods.ResimEkle(Film), parameter => true);

            FilmTarihGüncelle = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                var film = data[0] as Film;
                (data[1] as Salonlar).Serialize();
            }, parameter => true);

            FilmResimGüncelle = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                ExtensionMethods.ResimEkle(data[0] as Film);
                (data[1] as Salonlar).Serialize();
            }, parameter => true);

            WebFilmResimAktar = new RelayCommand<object>(parameter =>
            {
                try
                {
                    var data = parameter as object[];
                    var filename = $"{Guid.NewGuid()}.jpg";
                    using WebClient client = new();
                    if (File.Exists(MainWindowViewModel.xmldatapath))
                    {
                        var webimageadress = data[1] as string;
                        client.DownloadFile(new Uri(webimageadress), $"{Path.GetDirectoryName(MainWindowViewModel.xmldatapath)}\\{filename}");
                        var film = data[0] as Film;
                        film.ResimYolu = filename;
                        (data[2] as Salonlar).Serialize();
                    }
                }
                catch (Exception)
                {
                }
            }, parameter => true);

            FilmVideoGüncelle = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                ExtensionMethods.VideoEkle(data[0] as Film);
                (data[1] as Salonlar).Serialize();
            }, parameter => true);

            if (cvsfilmler is not null)
            {
                cvsfilmler.Filter += (s, e) => e.Accepted = (e.Item as Film)?.FilmSaati >= DateTime.Now;
            }

            PropertyChanged += FilmViewModel_PropertyChanged;
        }

        public Film Film
        {
            get => film;

            set
            {
                if (film != value)
                {
                    film = value;
                    OnPropertyChanged(nameof(Film));
                }
            }
        }

        public string FilmAramaMetni
        {
            get => filmAramaMetni;

            set
            {
                if (filmAramaMetni != value)
                {
                    filmAramaMetni = value;
                    OnPropertyChanged(nameof(FilmAramaMetni));
                }
            }
        }

        public ICommand FilmGirişiYap { get; }

        public ICommand FilmResimEkle { get; }

        public ICommand FilmResimGüncelle { get; }

        public ICommand FilmTarihGüncelle { get; }

        public string FilmTipi
        {
            get => filmTipi;

            set
            {
                if (filmTipi != value)
                {
                    filmTipi = value;
                    OnPropertyChanged(nameof(FilmTipi));
                }
            }
        }

        public ICommand FilmVideoEkle { get; }

        public ICommand FilmVideoGüncelle { get; }

        public string Saat
        {
            get => saat;

            set
            {
                if (saat != value)
                {
                    saat = value;
                    OnPropertyChanged(nameof(Saat));
                }
            }
        }

        public Salon SeçiliSalon
        {
            get => seçiliSalon;

            set
            {
                if (seçiliSalon != value)
                {
                    seçiliSalon = value;
                    OnPropertyChanged(nameof(SeçiliSalon));
                }
            }
        }

        public DateTime Tarih
        {
            get => tarih;

            set
            {
                if (tarih != value)
                {
                    tarih = value;
                    OnPropertyChanged(nameof(Tarih));
                }
            }
        }

        public bool TarihiGeçenFilmleriGizle
        {
            get => tarihiGeçenFilmleriGizle;

            set
            {
                if (tarihiGeçenFilmleriGizle != value)
                {
                    tarihiGeçenFilmleriGizle = value;
                    OnPropertyChanged(nameof(TarihiGeçenFilmleriGizle));
                }
            }
        }

        public ICommand WebFilmResimAktar { get; }

        private DateTime FilmSaatiniAl() => Tarih.AddHours(Convert.ToDouble(Saat.Split(':')[0])).AddMinutes(Convert.ToDouble(Saat.Split(':')[1]));

        private void FilmViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (cvsfilmler.View != null)
            {
                if (e.PropertyName is "TarihiGeçenFilmleriGizle")
                {
                    cvsfilmler.View.Filter = TarihiGeçenFilmleriGizle ? new Predicate<object>(film => (film as Film)?.FilmSaati > DateTime.Now) : null;
                }

                if (e.PropertyName is "FilmAramaMetni")
                {
                    if (!string.IsNullOrEmpty(FilmAramaMetni))
                    {
                        TarihiGeçenFilmleriGizle = false;
                    }
                    cvsfilmler.View.Filter = !string.IsNullOrEmpty(FilmAramaMetni) ? new Predicate<object>(film => (film as Film)?.Adı.Contains(FilmAramaMetni) == true) : null;
                }
            }
        }
    }
}