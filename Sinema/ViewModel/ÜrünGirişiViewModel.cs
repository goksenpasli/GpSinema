﻿using Extensions;
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
                    mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
                    Urun.ÜrünAdi = null;
                }
            }, parameter => !string.IsNullOrWhiteSpace(Urun.ÜrünAdi));

            ÜrünGüncelle = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                if (data[0] is Urun urun && data[1] is MainWindowViewModel mainWindowViewModel)
                {
                    urun.BirimFiyat = Math.Round(urun.BirimFiyat, 2);
                    mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
                }
            }, parameter => true);

            MusteriÜrünGirişiYap = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                Musteri Musteri = data[0] as Musteri;
                SalonViewModel salonViewModel = data[1] as SalonViewModel;
                Urun urun = data[2] as Urun;

                Urun seçiliurun = salonViewModel.Salonlar.Urunler.Urun.FirstOrDefault(z => z.Id == urun.Id);
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

                    salonViewModel.Salonlar.Serialize();
                }
                else
                {
                    MessageBox.Show("Depoda Yeterli Ürün Yok.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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