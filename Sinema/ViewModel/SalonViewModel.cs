using Extensions;
using Sinema.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;

namespace Sinema.ViewModel
{
    public class SalonViewModel : InpcBase
    {
        private Salon salon;

        private Salonlar salonlar;

        public SalonViewModel()
        {
            Salon = new Salon();
            Salonlar = new Salonlar
            {
                Salon = ExtensionMethods.SalonlarıYükle(),
            };
            Salonlar.Urunler.Urun = ÜrünleriYükle();
            Salonlar.KoltukTipleri.KoltukTipi = KoltukTipleriniYükle();

            SalonOluştur = new RelayCommand<object>(parameter =>
            {
                Salon salon = new()
                {
                    Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                    Adı = Salon.Adı,
                    Aktif = true,
                    EnKoltukSayı = Salon.EnKoltukSayı,
                    BoyKoltukSayı = Salon.BoyKoltukSayı
                };
                for (int i = 0; i < Salon.BoyKoltukSayı * Salon.EnKoltukSayı; i++)
                {
                    salon.Koltuklar.Add(new Koltuk() { KoltukEtkin = true, Görünür = true, No = i + 1 });
                }
                Salonlar.Salon.Add(salon);
                Salonlar.Serialize();
                Salon.Adı = null;
            }, parameter => !string.IsNullOrWhiteSpace(Salon.Adı));
        }

        public Salon Salon
        {
            get => salon;

            set
            {
                if (salon != value)
                {
                    salon = value;
                    OnPropertyChanged(nameof(Salon));
                }
            }
        }

        public Salonlar Salonlar
        {
            get => salonlar;

            set
            {
                if (salonlar != value)
                {
                    salonlar = value;
                    OnPropertyChanged(nameof(Salonlar));
                }
            }
        }

        public ICommand SalonOluştur { get; }

        private ObservableCollection<KoltukTipi> KoltukTipleriniYükle()
        {
            if (File.Exists(MainWindowViewModel.xmldatapath))
            {
                foreach (XElement item in XDocument.Load(MainWindowViewModel.xmldatapath).Descendants("KoltukTipleri").Descendants("KoltukTipi"))
                {
                    Salonlar.KoltukTipleri.KoltukTipi?.Add(item?.DeSerialize<KoltukTipi>());
                }
                return Salonlar.KoltukTipleri.KoltukTipi;
            }
            return new ObservableCollection<KoltukTipi>();
        }

        private ObservableCollection<Urun> ÜrünleriYükle()
        {
            if (File.Exists(MainWindowViewModel.xmldatapath))
            {
                foreach (XElement item in XDocument.Load(MainWindowViewModel.xmldatapath).Descendants("Urunler").Descendants("Urun"))
                {
                    Salonlar.Urunler.Urun?.Add(item?.DeSerialize<Urun>());
                }
                return Salonlar.Urunler.Urun;
            }
            return new ObservableCollection<Urun>();
        }
    }
}