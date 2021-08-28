using Extensions;
using Sinema.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sinema.ViewModel
{
    public class ÜrünGirişiViewModel : InpcBase
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
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                        ÜrünAdi = Urun.ÜrünAdi,
                        Adet = Urun.Adet,
                        BirimFiyat = Urun.BirimFiyat
                    };

                    mainWindowViewModel.SalonViewModel.Salonlar.Urunler.Urun.Add(ürün);
                    SalonViewModel.DatabaseSave.Execute(null);
                    Urun.ÜrünAdi = null;
                }
            }, parameter => !string.IsNullOrWhiteSpace(Urun.ÜrünAdi));

            ÜrünGüncelle = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                if (data[0] is Urun urun)
                {
                    urun.BirimFiyat = Math.Round(urun.BirimFiyat, 2);
                    SalonViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => true);

            MusteriÜrünGirişiYap = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                var Musteri = data[0] as Musteri;
                var salonViewModel = data[1] as SalonViewModel;
                var urun = data[2] as Urun;

                var seçiliurun = salonViewModel.Salonlar.Urunler.Urun.FirstOrDefault(z => z.Id == urun.Id);
                if (Musteri.Siparis.UrunAdet <= seçiliurun.Adet)
                {
                    Urun ürün = new()
                    {
                        Adet = Musteri.Siparis.UrunAdet,
                        BirimFiyat = urun.BirimFiyat,
                        ÜrünAdi = urun.ÜrünAdi,
                        Id = urun.Id
                    };

                    Musteri.Siparis.Tarih = DateTime.Now;
                    Musteri.Siparis.Urun.Add(ürün);
                    Musteri.Siparis.ToplamTutar = Musteri.Siparis.Urun.Sum(z => z.ToplamFiyat);
                    seçiliurun.Adet -= Musteri.Siparis.UrunAdet;

                    SalonViewModel.DatabaseSave.Execute(null);
                }
                else
                {
                    _ = MessageBox.Show("Depoda Yeterli Ürün Yok.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }, parameter => parameter is not null && (parameter as object[])?[2] is Urun && (parameter as object[])?[0] is Musteri);
        }

        public ICommand MusteriÜrünGirişiYap { get; }

        public Urun Urun
        {
            get => urun;

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

        public ICommand ÜrünGüncelle { get; }
    }
}