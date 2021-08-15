using Extensions;
using Sinema.Model;
using System;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;

namespace Sinema.ViewModel
{
    public class KoltukTipiGirişiViewModel : InpcBase
    {
        private Koltuk koltuk;

        private KoltukTipi koltukTipi;

        public KoltukTipiGirişiViewModel()
        {
            KoltukTipi = new KoltukTipi();
            KoltukTipiGirişiYap = new RelayCommand<object>(parameter =>
            {
                if (parameter is MainWindowViewModel mainWindowViewModel)
                {
                    KoltukTipi koltuktipi = new()
                    {
                        KoltukAçıklama = KoltukTipi.KoltukAçıklama,
                        KoltukFiyatı = KoltukTipi.KoltukFiyatı,
                        KoltukRenk = KoltukTipi.KoltukRenk,
                        Id = new Random(Guid.NewGuid().GetHashCode()).Next(1, int.MaxValue),
                    };

                    mainWindowViewModel.SalonViewModel.Salonlar.KoltukTipleri.KoltukTipi.Add(koltuktipi);
                    mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
                }
            }, parameter => !string.IsNullOrWhiteSpace(KoltukTipi.KoltukAçıklama));

            KoltukFiyatGüncelle = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is KoltukTipi koltukTipi && data[1] is MainWindowViewModel mainWindowViewModel)
                {
                    KoltukTipi.KoltukFiyatı = koltukTipi.KoltukFiyatı;
                    mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
                }
            }, parameter => true);

            KoltukAyarla = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Koltuk koltuk && data[1] is MainWindowViewModel mainWindowViewModel)
                {
                    var SeçiliKoltukTipi = data[2] as KoltukTipi;
                    if (SeçiliKoltukTipi is not null)
                    {
                        koltuk.SeçiliKoltukTipi = SeçiliKoltukTipi;
                        koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                    }
                    mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
                }
            }, parameter => true);

            KoltuklarıAyarla = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Salon salon && data[1] is MainWindowViewModel mainWindowViewModel && data[2] is KoltukTipi SeçiliKoltukTipi)
                {
                    KoltukTipiIdToBrushConverter.koltukrenkleri = XElement.Load(MainWindowViewModel.xmldatapath)?.Element("KoltukTipleri")?.Elements("KoltukTipi");

                    foreach (var koltuk in salon.Koltuklar)
                    {
                        if (salon.SeçiliKoltukDüzeni == 0)
                        {
                            koltuk.SeçiliKoltukTipi = SeçiliKoltukTipi;
                            koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                            if (salon.TopluKoltukGizle)
                            {
                                koltuk.Görünür = false;
                            }
                        }
                        if (salon?.SeçiliKoltukDüzeni == 1 && koltuk.No % 2 == 1)
                        {
                            koltuk.SeçiliKoltukTipi = SeçiliKoltukTipi;
                            koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                            if (salon.TopluKoltukGizle)
                            {
                                koltuk.Görünür = false;
                            }
                        }
                        if (salon?.SeçiliKoltukDüzeni == 2 && koltuk.No % 2 == 0)
                        {
                            koltuk.SeçiliKoltukTipi = SeçiliKoltukTipi;
                            koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                            if (salon.TopluKoltukGizle)
                            {
                                koltuk.Görünür = false;
                            }
                        }
                        if (salon?.SeçiliKoltukDüzeni == 3)
                        {
                            foreach (var koltukaralık in salon.Koltuklar.Where(z => z.No >= salon.KoltukAltAralık && z.No <= salon.KoltukÜstAralık))
                            {
                                koltukaralık.SeçiliKoltukTipi = SeçiliKoltukTipi;
                                koltukaralık.KoltukTipiId = SeçiliKoltukTipi.Id;
                                if (salon.TopluKoltukGizle)
                                {
                                    koltukaralık.Görünür = false;
                                }
                            }
                        }
                        if (salon?.SeçiliKoltukDüzeni == 4)
                        {
                            for (var j = 0; j <= (salon.KoltukÜstAralık - salon.KoltukAltAralık) / salon.EnKoltukSayı; j++)
                            {
                                for (var i = 0; i <= ((salon.KoltukÜstAralık - salon.KoltukAltAralık) % salon.EnKoltukSayı); i++)
                                {
                                    var koltukaralık = salon.Koltuklar.FirstOrDefault(z => z.No == (i + salon.KoltukAltAralık + j * salon.EnKoltukSayı));
                                    koltukaralık.SeçiliKoltukTipi = SeçiliKoltukTipi;
                                    koltukaralık.KoltukTipiId = SeçiliKoltukTipi.Id;
                                    if (salon.TopluKoltukGizle)
                                    {
                                        koltukaralık.Görünür = false;
                                    }
                                }
                            }
                        }
                    }

                    mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
                }
            }, parameter => parameter is not null && parameter is object[] data && data[0] is Salon salon && salon?.KoltukAltAralık <= salon?.KoltukÜstAralık && salon?.KoltukÜstAralık <= salon?.EnKoltukSayı * salon?.BoyKoltukSayı);

            TümKoltuklarıGöster = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Salon salon && data[1] is MainWindowViewModel mainWindowViewModel)
                {
                    foreach (var item in salon.Koltuklar)
                    {
                        if (!item.Görünür)
                        {
                            item.Görünür = true;
                        }
                    }
                    mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
                }
            }, parameter => true);
        }

        public ICommand ÇiftKoltuklarıAyarla { get; }

        public Koltuk Koltuk
        {
            get => koltuk;

            set
            {
                if (koltuk != value)
                {
                    koltuk = value;
                    OnPropertyChanged(nameof(Koltuk));
                }
            }
        }

        public ICommand KoltukAyarla { get; }

        public ICommand KoltukFiyatGüncelle { get; }

        public ICommand KoltuklarıAyarla { get; }

        public KoltukTipi KoltukTipi
        {
            get => koltukTipi;

            set
            {
                if (koltukTipi != value)
                {
                    koltukTipi = value;
                    OnPropertyChanged(nameof(KoltukTipi));
                }
            }
        }

        public ICommand KoltukTipiGirişiYap { get; }

        public ICommand TekKoltuklarıAyarla { get; }

        public ICommand TümKoltuklarıAyarla { get; }

        public ICommand TümKoltuklarıGöster { get; }
    }
}