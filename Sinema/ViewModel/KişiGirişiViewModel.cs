using Extensions;
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
                var koltuk = data[1] as Koltuk;
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
                    BiletFiyat = KoltukFiyatıAl(koltuk)
                };
                koltuk.KoltukDolu = false;
                koltuk.Müşteri.Add(musteri);
                koltuk.KoltukDolu = true;
                SalonViewModel.DatabaseSave.Execute(null);


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
                    SalonViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => true);

            MusteriTaşı = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                var Musteri = data[0] as Musteri;
                var kaynakkoltuk = data[1] as Koltuk;
                var salon = data[2] as Salon;
                var hedefkoltuk = salon.Koltuklar.FirstOrDefault(z => z.No == kaynakkoltuk.TaşınacakKoltukNo);

                if (!hedefkoltuk.Müşteri.Any(z => z.FilmId == Musteri.FilmId))
                {
                    hedefkoltuk.KoltukDolu = false;
                    hedefkoltuk.Müşteri.Add(Musteri);
                    hedefkoltuk.KoltukDolu = true;

                    kaynakkoltuk.KoltukDolu = true;
                    kaynakkoltuk.Müşteri.Remove(Musteri);
                    kaynakkoltuk.KoltukDolu = false;

                    SalonViewModel.DatabaseSave.Execute(null);
                }
                else
                {
                    MessageBox.Show($"Bu Filmde Taşınacak Koltukta {hedefkoltuk.Müşteri.FirstOrDefault(z => z.FilmId == Musteri.FilmId).Ad} Adlı Kişinin Kaydı Vardır.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }, parameter =>
            {
                if (parameter is not null)
                {
                    var koltuk = (parameter as object[])?[1] as Koltuk;
                    return koltuk?.No != koltuk?.TaşınacakKoltukNo;
                }
                return false;
            });
        }

        public ICommand MusteriGirişiYap { get; }

        public ICommand MusteriSil { get; }

        public ICommand MusteriTaşı { get; }

        private double KoltukFiyatıAl(Koltuk koltuk) => (double)(XElement.Load(MainWindowViewModel.xmldatapath)?.Element("KoltukTipleri")?.Elements("KoltukTipi")?.Where(z => (int)z.Attribute("Id") == koltuk.KoltukTipiId).Select(z => z.Attribute("KoltukFiyatı")).FirstOrDefault());
    }
}