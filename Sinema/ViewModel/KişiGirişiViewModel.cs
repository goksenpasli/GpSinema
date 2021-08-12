﻿using Extensions;
using Sinema.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Sinema.ViewModel
{
    public class KişiGirişiViewModel : InpcBase
    {
        public KişiGirişiViewModel()
        {
            MusteriGirişiYap = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                var koltuk = data[2] as Koltuk;
                if (koltuk.KoltukTipiId == 0)
                {
                    MessageBox.Show("Koltuk Tipi Ayarlanmamış Sağ Tıklayıp Veya Salondan Tüm Koltuk Tipini Ayarlayın.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                var Musteri = data[0] as Musteri;
                if (koltuk.Müşteri.Any(z => z.FilmId == Musteri.SeçiliFilm.Id))
                {
                    MessageBox.Show($"Bu Filme {koltuk.Müşteri.FirstOrDefault(z => z.FilmId == Musteri.SeçiliFilm.Id).Ad} Adlı Kişinin Kaydı Vardır.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                Musteri musteri = new()
                {
                    Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                    Ad = Musteri.Ad,
                    Soyad = Musteri.Soyad,
                    Yas = Musteri.Yas,
                    FilmId = Musteri.SeçiliFilm.Id,
                    BiletFiyat = Convert.ToDouble(KoltukFiyatıAl(koltuk))
                };
                koltuk.KoltukDolu = false;
                koltuk.Müşteri.Add(musteri);
                koltuk.KoltukDolu = true;
                var salonViewModel = data[1] as SalonViewModel;
                salonViewModel.Salonlar.Serialize();

                Musteri.Ad = null;
                Musteri.Soyad = null;
                Musteri.SeçiliFilm = null;
            }, parameter =>
            {
                if (parameter is not null)
                {
                    var data = parameter as object[];
                    var Musteri = data[0] as Musteri;
                    return !string.IsNullOrWhiteSpace(Musteri?.Soyad) && !string.IsNullOrWhiteSpace(Musteri?.Ad) && Musteri?.SeçiliFilm is not null;
                }
                return false;
            });

            MusteriSil = new RelayCommand<object>(parameter =>
            {
                if (MessageBox.Show("Seçili Müşteriyi Silmek İstiyor Musun?", "SİNEMA", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    var data = parameter as object[];
                    var Musteri = data[0] as Musteri;
                    var salonViewModel = data[1] as SalonViewModel;
                    var koltuk = data[2] as Koltuk;

                    foreach (var müşterisipariş in Musteri?.Siparis?.Urun)
                    {
                        foreach (var depoürün in salonViewModel?.Salonlar?.Urunler?.Urun)
                        {
                            if (müşterisipariş.Id == depoürün.Id)
                            {
                                depoürün.Adet += müşterisipariş.Adet;
                            }
                        }
                    }

                    koltuk.KoltukDolu = true;
                    koltuk.Müşteri.Remove(Musteri);
                    koltuk.KoltukDolu = false;
                    salonViewModel.Salonlar.Serialize();
                }
            }, parameter => true);

            MusteriTaşı = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                var Musteri = data[0] as Musteri;
                var kaynakkoltuk = data[2] as Koltuk;
                var salon = data[3] as Salon;
                var hedefkoltuk = salon.Koltuklar.FirstOrDefault(z => z.No == kaynakkoltuk.TaşınacakKoltukNo);

                if (!hedefkoltuk.Müşteri.Any(z => z.FilmId == Musteri.FilmId))
                {
                    hedefkoltuk.KoltukDolu = false;
                    hedefkoltuk.Müşteri.Add(Musteri);
                    hedefkoltuk.KoltukDolu = true;

                    kaynakkoltuk.KoltukDolu = true;
                    kaynakkoltuk.Müşteri.Remove(Musteri);
                    kaynakkoltuk.KoltukDolu = false;

                    var salonViewModel = data[1] as SalonViewModel;
                    salonViewModel.Salonlar.Serialize();
                }
                else
                {
                    MessageBox.Show($"Bu Filmde Taşınacak Koltukta {hedefkoltuk.Müşteri.FirstOrDefault(z => z.FilmId == Musteri.FilmId).Ad} Adlı Kişinin Kaydı Vardır.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }, parameter => true);
        }

        public ICommand MusteriGirişiYap { get; }

        public ICommand MusteriSil { get; }

        public ICommand MusteriTaşı { get; }

        private string KoltukFiyatıAl(Koltuk koltuk)
        {
            return XElement.Load(MainWindowViewModel.xmldatapath)?.Element("KoltukTipleri")?.Elements("KoltukTipi")?.Where(z => (int)z.Attribute("Id") == koltuk.KoltukTipiId).Select(z => z.Attribute("KoltukFiyatı").Value).FirstOrDefault();
        }
    }
}