using Extensions;
using Sinema.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using ExtensionMethods = Sinema.ViewModel.ExtensionMethods;

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

        private double ilaveKoltukSayısı;

        private double koltukAltAralık = 1;

        private ObservableCollection<Koltuk> koltuklar = new();

        private double koltukÜstAralık = 1;

        private IEnumerable<IGrouping<int, int>> salonEnBoyYapısı;

        private ObservableCollection<string> salonHarfleri = new();

        private IEnumerable<int> salonÖnizleme;

        private Film seçiliFilm;

        private int seçiliKoltukDüzeni;

        private Salon seçiliSalon;

        private IGrouping<int, int> seçiliSalonKoltukGrubu;

        private bool topluKoltukGizle;
        private ObservableCollection<string> salonSayıları;

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
                    OnPropertyChanged(nameof(SalonEnBoyYapısı));
                    OnPropertyChanged(nameof(SalonÖnizleme));
                    OnPropertyChanged(nameof(SalonSayıları));
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
                    OnPropertyChanged(nameof(SalonEnBoyYapısı));
                    OnPropertyChanged(nameof(SalonHarfleri));
                    OnPropertyChanged(nameof(SalonÖnizleme));
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

        [XmlIgnore]
        public double İlaveKoltukSayısı
        {
            get => ilaveKoltukSayısı;

            set
            {
                if (ilaveKoltukSayısı != value)
                {
                    ilaveKoltukSayısı = value;
                    OnPropertyChanged(nameof(İlaveKoltukSayısı));
                    OnPropertyChanged(nameof(SalonEnBoyYapısı));
                }
            }
        }

        [XmlIgnore]
        public double KoltukAltAralık
        {
            get => koltukAltAralık;

            set
            {
                if (koltukAltAralık != value)
                {
                    koltukAltAralık = value;
                    OnPropertyChanged(nameof(KoltukAltAralık));
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
        public double KoltukÜstAralık
        {
            get => koltukÜstAralık;

            set
            {
                if (koltukÜstAralık != value)
                {
                    koltukÜstAralık = value;
                    OnPropertyChanged(nameof(KoltukÜstAralık));
                }
            }
        }

        [XmlIgnore]
        public IEnumerable<IGrouping<int, int>> SalonEnBoyYapısı
        {
            get
            {
                salonEnBoyYapısı = ExtensionMethods.GroupBy(((int)İlaveKoltukSayısı + (EnKoltukSayı * BoyKoltukSayı)).Bölenler(), 2);
                return salonEnBoyYapısı;
            }

            set
            {
                if (salonEnBoyYapısı != value)
                {
                    salonEnBoyYapısı = value;
                    OnPropertyChanged(nameof(SalonEnBoyYapısı));
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<string> SalonHarfleri
        {
            get
            {
                salonHarfleri = ExtensionMethods.SalonHarfleri(EnKoltukSayı);
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

        public ObservableCollection<string> SalonSayıları
        {
            get
            {
                salonSayıları = new();
                for (int i = 1; i <= BoyKoltukSayı; i++)
                {
                    salonSayıları.Add(i.ToString());
                }
                return salonSayıları;
            }

            set
            {
                if (salonSayıları != value)
                {
                    salonSayıları = value;
                    OnPropertyChanged(nameof(SalonSayıları));
                }
            }
        }

        [XmlIgnore]
        public IEnumerable<int> SalonÖnizleme
        {
            get
            {
                salonÖnizleme = Enumerable.Range(1, BoyKoltukSayı * EnKoltukSayı);
                return salonÖnizleme;
            }

            set
            {
                if (salonÖnizleme != value)
                {
                    salonÖnizleme = value;
                    OnPropertyChanged(nameof(SalonÖnizleme));
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
        public int SeçiliKoltukDüzeni
        {
            get => seçiliKoltukDüzeni;

            set
            {
                if (seçiliKoltukDüzeni != value)
                {
                    seçiliKoltukDüzeni = value;
                    OnPropertyChanged(nameof(SeçiliKoltukDüzeni));
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

        [XmlIgnore]
        public IGrouping<int, int> SeçiliSalonKoltukGrubu
        {
            get => seçiliSalonKoltukGrubu;

            set
            {
                if (seçiliSalonKoltukGrubu != value)
                {
                    seçiliSalonKoltukGrubu = value;
                    OnPropertyChanged(nameof(SeçiliSalonKoltukGrubu));
                }
            }
        }

        [XmlIgnore]
        public bool TopluKoltukGizle
        {
            get => topluKoltukGizle;

            set
            {
                if (topluKoltukGizle != value)
                {
                    topluKoltukGizle = value;
                    OnPropertyChanged(nameof(TopluKoltukGizle));
                }
            }
        }
    }
}