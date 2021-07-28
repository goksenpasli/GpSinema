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
            }, parameter => !string.IsNullOrWhiteSpace(KoltukTipi.KoltukRenk) && !string.IsNullOrWhiteSpace(KoltukTipi.KoltukAçıklama));

            KoltukAyarla = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                Koltuk koltuk = data[0] as Koltuk;
                MainWindowViewModel mainWindowViewModel = data[1] as MainWindowViewModel;
                KoltukTipi SeçiliKoltukTipi = data[2] as KoltukTipi;
                if (SeçiliKoltukTipi is not null)
                {
                    koltuk.SeçiliKoltukTipi = SeçiliKoltukTipi;
                    koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                }

                mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
            }, parameter => true);

            KoltuklarıAyarla = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                Salon salon = data[0] as Salon;
                MainWindowViewModel mainWindowViewModel = data[1] as MainWindowViewModel;
                KoltukTipi SeçiliKoltukTipi = data[2] as KoltukTipi;
                KoltukTipiIdToBrushConverter.koltukrenkleri = XElement.Load(MainWindowViewModel.xmldatapath)?.Element("KoltukTipleri")?.Elements("KoltukTipi");

                foreach (Koltuk koltuk in salon.Koltuklar)
                {
                    if (salon.SeçiliKoltukDüzeni == 0)
                    {
                        koltuk.SeçiliKoltukTipi = SeçiliKoltukTipi;
                        koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                    }
                    if (salon.SeçiliKoltukDüzeni == 1 && koltuk.No % 2 == 1)
                    {
                        koltuk.SeçiliKoltukTipi = SeçiliKoltukTipi;
                        koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                    }
                    if (salon.SeçiliKoltukDüzeni == 2 && koltuk.No % 2 == 0)
                    {
                        koltuk.SeçiliKoltukTipi = SeçiliKoltukTipi;
                        koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                    }
                    if (salon.SeçiliKoltukDüzeni == 3)
                    {
                        foreach (Koltuk koltukaralık in salon.Koltuklar.Where(z => z.No >= salon.KoltukAltAralık && z.No <= salon.KoltukÜstAralık))
                        {
                            koltukaralık.SeçiliKoltukTipi = SeçiliKoltukTipi;
                            koltukaralık.KoltukTipiId = SeçiliKoltukTipi.Id;
                        }
                    }
                    if (salon.SeçiliKoltukDüzeni == 4)
                    {
                        for (int j = 0; j <= (salon.KoltukÜstAralık - salon.KoltukAltAralık) / salon.EnKoltukSayı; j++)
                        {
                            for (int i = 0; i <= ((salon.KoltukÜstAralık - salon.KoltukAltAralık) % salon.EnKoltukSayı); i++)
                            {
                                Koltuk koltukaralık = salon.Koltuklar.FirstOrDefault(z => z.No == (i + salon.KoltukAltAralık + j * salon.EnKoltukSayı));
                                koltukaralık.SeçiliKoltukTipi = SeçiliKoltukTipi;
                                koltukaralık.KoltukTipiId = SeçiliKoltukTipi.Id;
                            }
                        }
                    }
                }

                mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
            }, parameter =>
            {
                if (parameter is not null)
                {
                    object[] data = parameter as object[];
                    Salon salon = data[0] as Salon;
                    return salon.KoltukAltAralık <= salon.KoltukÜstAralık && salon.KoltukÜstAralık <= salon.EnKoltukSayı * salon.BoyKoltukSayı;
                }
                return false;
            });

            TümKoltuklarıGöster = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                Salon salon = data[0] as Salon;
                MainWindowViewModel mainWindowViewModel = data[1] as MainWindowViewModel;
                foreach (Koltuk item in salon.Koltuklar)
                {
                    if (!item.Görünür)
                    {
                        item.Görünür = true;
                    }
                }
                mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
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