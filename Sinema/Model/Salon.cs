using Extensions;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "Salon")]
    public class Salon : InpcBase
    {
        private string adı;

        private bool aktif;

        private int boyKoltukSayı = 1;

        private int enKoltukSayı = 1;

        private ObservableCollection<Film> film = new();

        private int ıd;

        private ObservableCollection<Koltuk> koltuklar = new();

        private ObservableCollection<string> salonHarfleri = new();

        private Film seçiliFilm;

        private Salon seçiliSalon;

        [XmlAttribute(AttributeName = "Adı")]
        public string Adı
        {
            get => adı;

            set
            {
                if (adı != value)
                {
                    adı = value;
                    OnPropertyChanged(nameof(Adı));
                }
            }
        }

        [XmlAttribute(AttributeName = "Aktif")]
        public bool Aktif
        {
            get => aktif;

            set
            {
                if (aktif != value)
                {
                    aktif = value;
                    OnPropertyChanged(nameof(Aktif));
                }
            }
        }

        [XmlAttribute(AttributeName = "BoyKoltukSayı")]
        public int BoyKoltukSayı
        {
            get => boyKoltukSayı;

            set
            {
                if (boyKoltukSayı != value)
                {
                    boyKoltukSayı = value;
                    OnPropertyChanged(nameof(BoyKoltukSayı));
                }
            }
        }

        [XmlAttribute(AttributeName = "EnKoltukSayı")]
        public int EnKoltukSayı
        {
            get => enKoltukSayı;

            set
            {
                if (enKoltukSayı != value)
                {
                    enKoltukSayı = value;
                    OnPropertyChanged(nameof(EnKoltukSayı));
                }
            }
        }

        [XmlElement(ElementName = "Film")]
        public ObservableCollection<Film> Film
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

        [XmlAttribute(AttributeName = "Id")]
        public int Id
        {
            get => ıd;

            set
            {
                if (ıd != value)
                {
                    ıd = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public ObservableCollection<Koltuk> Koltuklar
        {
            get => koltuklar;

            set
            {
                if (koltuklar != value)
                {
                    koltuklar = value;
                    OnPropertyChanged(nameof(Koltuklar));
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<string> SalonHarfleri
        {
            get
            {
                salonHarfleri = ViewModel.ExtensionMethods.SalonHarfleri(EnKoltukSayı);
                return salonHarfleri;
            }

            set
            {
                if (salonHarfleri != value)
                {
                    salonHarfleri = value;
                    OnPropertyChanged(nameof(SalonHarfleri));
                }
            }
        }

        [XmlIgnore]
        public Film SeçiliFilm
        {
            get => seçiliFilm;

            set
            {
                if (seçiliFilm != value)
                {
                    seçiliFilm = value;
                    OnPropertyChanged(nameof(SeçiliFilm));
                }
            }
        }

        [XmlIgnore]
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
    }
}