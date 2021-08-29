using Extensions;
using Sinema.Model;
using System;
using System.Linq;
using System.Windows.Input;

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
                    SalonViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => !string.IsNullOrWhiteSpace(KoltukTipi.KoltukAçıklama));

            KoltukFiyatGüncelle = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is KoltukTipi koltukTipi)
                {
                    KoltukTipi.KoltukFiyatı = koltukTipi.KoltukFiyatı;
                    SalonViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => true);

            KoltukAyarla = new RelayCommand<object>(parameter => SalonViewModel.DatabaseSave.Execute(null), parameter => true);

            KoltuklarıAyarla = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Salon salon && data[1] is KoltukTipi SeçiliKoltukTipi)
                {
                    foreach (var koltuk in salon.Koltuklar)
                    {
                        if (salon.SeçiliKoltukDüzeni == 0)
                        {
                            koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                            if (salon.TopluKoltukGizle)
                            {
                                koltuk.Görünür = false;
                            }
                        }
                        if (salon?.SeçiliKoltukDüzeni == 1 && koltuk.No % 2 == 1)
                        {
                            koltuk.KoltukTipiId = SeçiliKoltukTipi.Id;
                            if (salon.TopluKoltukGizle)
                            {
                                koltuk.Görünür = false;
                            }
                        }
                        if (salon?.SeçiliKoltukDüzeni == 2 && koltuk.No % 2 == 0)
                        {
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
                                    koltukaralık.KoltukTipiId = SeçiliKoltukTipi.Id;
                                    if (salon.TopluKoltukGizle)
                                    {
                                        koltukaralık.Görünür = false;
                                    }
                                }
                            }
                        }
                    }

                    SalonViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => parameter is not null && parameter is object[] data && data[0] is Salon salon && salon?.KoltukAltAralık <= salon?.KoltukÜstAralık && salon?.KoltukÜstAralık <= salon?.EnKoltukSayı * salon?.BoyKoltukSayı);

            TümKoltuklarıGöster = new RelayCommand<object>(parameter =>
            {
                if (parameter is object[] data && data[0] is Salon salon)
                {
                    foreach (var item in salon.Koltuklar)
                    {
                        if (!item.Görünür)
                        {
                            item.Görünür = true;
                        }
                    }
                    SalonViewModel.DatabaseSave.Execute(null);
                }
            }, parameter => true);
        }

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

        public ICommand TümKoltuklarıGöster { get; }
    }
}