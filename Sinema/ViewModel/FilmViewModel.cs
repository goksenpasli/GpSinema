using Extensions;
using Sinema.Model;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Input;

namespace Sinema.ViewModel
{
    public class FilmViewModel : InpcBase
    {
        private Film film;

        private string filmTipi;

        private string saat = "00:00";

        private Salon seçiliSalon;

        private DateTime tarih = DateTime.Today;

        public FilmViewModel()
        {
            Film = new Film();

            FilmGirişiYap = new RelayCommand<object>(parameter =>
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
                    FilmSaati = Tarih.AddHours(Convert.ToDouble(Saat.Split(':')[0])).AddMinutes(Convert.ToDouble(Saat.Split(':')[1])),
                };

                SeçiliSalon?.Film.Add(film);
                (parameter as Salonlar).Serialize();
            }, parameter => SeçiliSalon is not null && !string.IsNullOrWhiteSpace(Film.Adı) && DateTime.TryParseExact(Saat, "H:m", new CultureInfo("tr-TR"), DateTimeStyles.None, out _));

            FilmVideoEkle = new RelayCommand<object>(parameter => ExtensionMethods.VideoEkle(Film), parameter => true);

            FilmResimEkle = new RelayCommand<object>(parameter => ExtensionMethods.ResimEkle(Film), parameter => true);

            FilmResimGüncelle = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                ExtensionMethods.ResimEkle(data[0] as Film);
                (data[1] as Salonlar).Serialize();
            }, parameter => true);

            WebFilmResimAktar = new RelayCommand<object>(parameter =>
            {
                try
                {
                    object[] data = parameter as object[];
                    string filename = $"{Guid.NewGuid()}.jpg";
                    using (WebClient client = new())
                    {
                        if (File.Exists(MainWindowViewModel.xmldatapath))
                        {
                            string webimageadress = data[1] as string;
                            client.DownloadFileAsync(new Uri(webimageadress), $"{Path.GetDirectoryName(MainWindowViewModel.xmldatapath)}\\{filename}");
                        }
                    }
                    Film film = data[0] as Film;
                    film.ResimYolu = filename;
                }
                catch (Exception)
                {
                }
            }, parameter => true);

            FilmVideoGüncelle = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                ExtensionMethods.VideoEkle(data[0] as Film);
                (data[1] as Salonlar).Serialize();
            }, parameter => true);
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

        public ICommand FilmGirişiYap { get; }

        public ICommand FilmResimEkle { get; }

        public ICommand FilmResimGüncelle { get; }

        public ICommand WebFilmResimAktar { get; }

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
    }
}