using Extensions;
using Sinema.Model;
using System;
using System.Linq;
using System.Windows.Input;

namespace Sinema.ViewModel
{
    public class ÜrünGirişiViewModel:InpcBase
    {
        private Urun urun;

        public ÜrünGirişiViewModel()
        {
            Urun = new Urun();
            ÜrünGir = new RelayCommand<object>(parameter =>
            {
                if (parameter is MainWindowViewModel mainWindowViewModel)
                {
                    Urun ürün = new()
                    {
                        ÜrünAdi = Urun.ÜrünAdi,
                        Adet = Urun.Adet,
                        BirimFiyat = Urun.BirimFiyat
                    };

                    mainWindowViewModel.SalonViewModel.Salonlar.Urunler.Urun.Add(ürün);
                    mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
                }
            }, parameter => !string.IsNullOrWhiteSpace(Urun.ÜrünAdi));

            MusteriÜrünGirişiYap = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                Musteri Musteri = data[0] as Musteri;
                SalonViewModel salonViewModel = data[1] as SalonViewModel;
                Urun urun = data[2] as Urun;

                Urun ürün = new()
                {
                    Adet = Musteri.Siparis.UrunAdet,
                    BirimFiyat = urun.BirimFiyat,
                    ÜrünAdi = urun.ÜrünAdi
                };

                Musteri.Siparis.Tarih = DateTime.Now;
                Musteri.Siparis.Urun.Add(ürün);
                Musteri.Siparis.ToplamTutar = Musteri.Siparis.Urun.Sum(z => z.ToplamFiyat);

                salonViewModel.Salonlar.Serialize();
            }, parameter => parameter is not null && (parameter as object[])?[2] is Urun && (parameter as object[])?[0] is Musteri);
        }

        public ICommand MusteriÜrünGirişiYap { get; }

        public Urun Urun
        {
            get { return urun; }

            set
            {
                if (urun != value)
                {
                    urun = value;
                    OnPropertyChanged(nameof(Urun));
                }
            }
        }

        public ICommand ÜrünGir { get; }
    }
}