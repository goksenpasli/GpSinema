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
                object[] data = parameter as object[];
                Koltuk koltuk = data[2] as Koltuk;
                if (koltuk.KoltukTipiId == 0)
                {
                    MessageBox.Show("Koltuk Tipi Ayarlanmamış Sağ Tıklayıp Veya Salondan Tüm Koltuk Tipini Ayarlayın.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                Film film = data[3] as Film;
                if (koltuk.Müşteri.Any(z => z.FilmId == film.Id))
                {
                    MessageBox.Show($"Bu Filme {koltuk.Müşteri.FirstOrDefault(z => z.FilmId == film.Id).Ad} Adlı Kişinin Kaydı Vardır.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                string koltukfiyatı = XElement.Load(MainWindowViewModel.xmldatapath)?.Element("KoltukTipleri")?.Elements("KoltukTipi")?.Where(z => (int)z.Attribute("Id") == koltuk.KoltukTipiId).Select(z => z.Attribute("KoltukFiyatı").Value).FirstOrDefault();

                Musteri Musteri = data[0] as Musteri;
                Musteri musteri = new()
                {
                    Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                    Ad = Musteri.Ad,
                    Soyad = Musteri.Soyad,
                    Yas = Musteri.Yas,
                    FilmId = film.Id,
                    BiletFiyat = Convert.ToDouble(koltukfiyatı)
                };
                koltuk.KoltukDolu = false;
                koltuk.Müşteri.Add(musteri);
                koltuk.KoltukDolu = true;
                SalonViewModel salonViewModel = data[1] as SalonViewModel;
                salonViewModel.Salonlar.Serialize();
            }, parameter =>
            {
                if (parameter is not null)
                {
                    object[] data = parameter as object[];
                    Musteri Musteri = data[0] as Musteri;
                    Film film = data[3] as Film;
                    return !string.IsNullOrWhiteSpace(Musteri?.Soyad) && !string.IsNullOrWhiteSpace(Musteri?.Ad) && film is not null;
                }
                return false;
            });

            MusteriSil = new RelayCommand<object>(parameter =>
            {
                if (MessageBox.Show("Seçili Müşteriyi Silmek İstiyor Musun?", "SİNEMA", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    object[] data = parameter as object[];
                    Musteri Musteri = data[0] as Musteri;
                    SalonViewModel salonViewModel = data[1] as SalonViewModel;
                    Koltuk koltuk = data[2] as Koltuk;

                    foreach (Urun müşterisipariş in Musteri?.Siparis?.Urun)
                    {
                        foreach (Urun depoürün in salonViewModel?.Salonlar?.Urunler?.Urun)
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
                object[] data = parameter as object[];
                Musteri Musteri = data[0] as Musteri;
                Koltuk kaynakkoltuk = data[2] as Koltuk;
                Salon salon = data[3] as Salon;
                Koltuk hedefkoltuk = salon.Koltuklar.FirstOrDefault(z => z.No == kaynakkoltuk.TaşınacakKoltukNo);

                if (!hedefkoltuk.Müşteri.Any(z => z.FilmId == Musteri.FilmId))
                {
                    hedefkoltuk.KoltukDolu = false;
                    hedefkoltuk.Müşteri.Add(Musteri);
                    hedefkoltuk.KoltukDolu = true;

                    kaynakkoltuk.KoltukDolu = true;
                    kaynakkoltuk.Müşteri.Remove(Musteri);
                    kaynakkoltuk.KoltukDolu = false;

                    SalonViewModel salonViewModel = data[1] as SalonViewModel;
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
    }
}