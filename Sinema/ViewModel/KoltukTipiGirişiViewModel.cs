﻿using Extensions;
using Sinema.Model;
using System;
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

            TümKoltuklarıAyarla = new RelayCommand<object>(parameter =>
            {
                object[] data = parameter as object[];
                Salon salon = data[0] as Salon;
                MainWindowViewModel mainWindowViewModel = data[1] as MainWindowViewModel;
                KoltukTipi SeçiliKoltukTipi = data[2] as KoltukTipi;
                KoltukTipiIdToBrushConverter.koltukrenkleri = XElement.Load(MainWindowViewModel.xmldatapath)?.Element("KoltukTipleri")?.Elements("KoltukTipi");
                foreach (Koltuk item in salon.Koltuklar)
                {
                    item.SeçiliKoltukTipi = SeçiliKoltukTipi;
                    item.KoltukTipiId = SeçiliKoltukTipi.Id;
                }

                mainWindowViewModel.SalonViewModel.Salonlar.Serialize();
            }, parameter => true);
        }

        public Koltuk Koltuk
        {
            get { return koltuk; }

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

        public KoltukTipi KoltukTipi
        {
            get { return koltukTipi; }

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

        public ICommand TümKoltuklarıAyarla { get; }
    }
}