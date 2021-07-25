using Extensions;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "Salonlar")]
    public class Salonlar : InpcBase
    {
        private KoltukTipleri koltukTipleri = new();

        private ObservableCollection<Salon> salon = new();

        private Urunler urunler = new();

        [XmlElement(ElementName = "KoltukTipleri")]
        public KoltukTipleri KoltukTipleri
        {
            get => koltukTipleri;

            set
            {
                if (koltukTipleri != value)
                {
                    koltukTipleri = value;
                    OnPropertyChanged(nameof(KoltukTipleri));
                }
            }
        }

        [XmlElement(ElementName = "Salon")]
        public ObservableCollection<Salon> Salon
        {
            get => salon;

            set
            {
                if (salon != value)
                {
                    salon = value;
                    OnPropertyChanged(nameof(Salon));
                }
            }
        }

        [XmlElement(ElementName = "Urunler")]
        public Urunler Urunler
        {
            get => urunler;

            set
            {
                if (urunler != value)
                {
                    urunler = value;
                    OnPropertyChanged(nameof(Urunler));
                }
            }
        }
    }
}