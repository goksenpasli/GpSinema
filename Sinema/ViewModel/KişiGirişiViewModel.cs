using Extensions;
using Sinema.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Sinema.ViewModel
{
    public class KişiGirişiViewModel : InpcBase
    {
        private Regex regex;

        private Koltuk topluGirişBaşlangıçKoltuk;

        private Koltuk topluGirişBitişKoltuk;

        private string topluGirişİsimListesi;

        public KişiGirişiViewModel()
        {
            MusteriGirişiYap = new RelayCommand<object>(parameter =>
            {
                var data = parameter as object[];
                var koltuk = data[1] as Koltuk;
                if (koltuk.KoltukTipiId == 0)
                {
                    _ = MessageBox.Show("Koltuk Tipi Ayarlanmamış Sağ Tıklayıp Veya Salondan Tüm Koltuk Tipini Ayarlayın.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                var Musteri = data[0] as Musteri;
                if (koltuk.Müşteri.Any(z => z.FilmId == Musteri.SeçiliFilm.Id))
                {
                    _ = MessageBox.Show($"Bu Filme {koltuk.Müşteri.FirstOrDefault(z => z.FilmId == Musteri.SeçiliFilm.Id).Ad} Adlı Kişinin Kaydı Vardır.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                TopluGirişBaşlangıçKoltuk = null;
                TopluGirişBitişKoltuk = null;
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

            TopluMusteriGirişiYap = new RelayCommand<object>(parameter =>
            {
                if (parameter is Salon salon)
                {
                    var j = 0;
                    double toplamtutar = 0;
                    var koltukdolu = false;
                    if (salon?.SeçiliSalon?.Koltuklar?.Any(z => z.No >= TopluGirişBaşlangıçKoltuk.No && z.No <= TopluGirişBitişKoltuk.No && z.KoltukTipiId == 0) == true)
                    {
                        _ = MessageBox.Show("Bu Salonda Halen Koltuk Tipi Ayarlanmamış Koltuk Var Sağ Tıklayıp Ayarlayın Veya Salondan Tüm Koltuk Tipini Ayarlayın.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                    foreach (var _ in (salon?.SeçiliSalon.Koltuklar).Where(koltuk => koltuk.No >= TopluGirişBaşlangıçKoltuk.No && koltuk.No <= TopluGirişBitişKoltuk.No && koltuk.Müşteri.Any(z => z.FilmId == salon.SeçiliFilm.Id)))
                    {
                        koltukdolu = true;
                    }

                    if (koltukdolu)
                    {
                        _ = MessageBox.Show("Belirtilen Aralıkta Dolu Koltuk Var Seçimi Değiştirin.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                    if (salon?.SeçiliSalon?.Koltuklar?.Any(z => z.No >= TopluGirişBaşlangıçKoltuk.No && z.No <= TopluGirişBitişKoltuk.No && z.KoltukTipiId == 0) == true)
                    {
                        _ = MessageBox.Show("Bu Salonda Halen Koltuk Tipi Ayarlanmamış Koltuk Var Sağ Tıklayıp Ayarlayın Veya Salondan Tüm Koltuk Tipini Ayarlayın.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                    var isimler = TopluGirişİsimListesi?.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    for (var i = TopluGirişBaşlangıçKoltuk.No; i <= TopluGirişBitişKoltuk.No; i++)
                    {
                        var koltuk = salon.SeçiliSalon.Koltuklar.FirstOrDefault(z => z.No == i);
                        Musteri musteri = new()
                        {
                            Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                            Ad = isimler[j]?.Split(' ')[0],
                            Soyad = isimler[j]?.Split(' ')[1],
                            Yas = 18,
                            FilmId = salon.SeçiliFilm.Id,
                            BiletFiyat = KoltukFiyatıAl(koltuk)
                        };

                        toplamtutar += musteri.BiletFiyat;
                        koltuk.KoltukDolu = false;
                        koltuk.Müşteri.Add(musteri);
                        koltuk.KoltukDolu = true;
                        j++;
                    }
                    SalonViewModel.DatabaseSave.Execute(null);

                    TopluGirişİsimListesi = null;
                    TopluGirişBaşlangıçKoltuk = null;
                    TopluGirişBitişKoltuk = null;

                    _ = MessageBox.Show($"Toplam Tahsil Edilecek Tutar {toplamtutar:C}", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }, parameter =>
            {
                regex = new Regex("(^.* .*$)", RegexOptions.Multiline);
                return parameter is Salon salon &&
                !string.IsNullOrWhiteSpace(TopluGirişİsimListesi) &&
                salon.SeçiliFilm is not null && TopluGirişBaşlangıçKoltuk is not null && TopluGirişBitişKoltuk is not null &&
                TopluGirişBaşlangıçKoltuk.No < TopluGirişBitişKoltuk.No &&
                regex.Matches(TopluGirişİsimListesi).Count == TopluGirişBitişKoltuk.No - TopluGirişBaşlangıçKoltuk.No + 1;
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
                    _ = koltuk.Müşteri.Remove(Musteri);
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
                    _ = kaynakkoltuk.Müşteri.Remove(Musteri);
                    kaynakkoltuk.KoltukDolu = false;

                    SalonViewModel.DatabaseSave.Execute(null);
                }
                else
                {
                    _ = MessageBox.Show($"Bu Filmde Taşınacak Koltukta {hedefkoltuk.Müşteri.FirstOrDefault(z => z.FilmId == Musteri.FilmId).Ad} Adlı Kişinin Kaydı Vardır.", "SİNEMA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        public Koltuk TopluGirişBaşlangıçKoltuk
        {
            get => topluGirişBaşlangıçKoltuk;

            set
            {
                if (topluGirişBaşlangıçKoltuk != value)
                {
                    topluGirişBaşlangıçKoltuk = value;
                    OnPropertyChanged(nameof(TopluGirişBaşlangıçKoltuk));
                }
            }
        }

        public Koltuk TopluGirişBitişKoltuk
        {
            get => topluGirişBitişKoltuk;

            set
            {
                if (topluGirişBitişKoltuk != value)
                {
                    topluGirişBitişKoltuk = value;
                    OnPropertyChanged(nameof(TopluGirişBitişKoltuk));
                }
            }
        }

        public string TopluGirişİsimListesi
        {
            get => topluGirişİsimListesi;

            set
            {
                if (topluGirişİsimListesi != value)
                {
                    topluGirişİsimListesi = value;
                    OnPropertyChanged(nameof(TopluGirişİsimListesi));
                }
            }
        }

        public ICommand TopluMusteriGirişiYap { get; }

        private double KoltukFiyatıAl(Koltuk koltuk) => (double)(XElement.Load(MainWindowViewModel.xmldatapath)?.Element("KoltukTipleri")?.Elements("KoltukTipi")?.Where(z => (int)z.Attribute("Id") == koltuk.KoltukTipiId).Select(z => z.Attribute("KoltukFiyatı")).FirstOrDefault());
    }
}