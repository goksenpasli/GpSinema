using Extensions;
using Microsoft.Win32;
using Sinema.Model;
using System;
using System.Globalization;
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
                    FilmSaati = Tarih.AddHours(Convert.ToDouble(Saat.Split(':')[0])).AddMinutes(Convert.ToDouble(Saat.Split(':')[1])),
                };

                SeçiliSalon?.Film.Add(film);
                (parameter as Salonlar).Serialize();
            }, parameter => SeçiliSalon is not null && !string.IsNullOrWhiteSpace(Film.Adı) && DateTime.TryParseExact(Saat, "H:m", new CultureInfo("tr-TR"), DateTimeStyles.None, out _));

            FilmVideoEkle = new RelayCommand<object>(parameter =>
            {
                OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Video Dosyaları (*.mp4)|*.mp4" };
                if (openFileDialog.ShowDialog() == true)
                {
                    Film.VideoYolu = openFileDialog.FileName;
                }
            }, parameter => true);

            FilmResimEkle = new RelayCommand<object>(parameter => ExtensionMethods.ResimEkle(Film), parameter => true);

            FilmResimGüncelle = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                ExtensionMethods.ResimEkle(data[0] as Film);
                (data[1] as Salonlar).Serialize();
            }, parameter => true);
        }

        public Film Film
        {
            get { return film; }

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