using Extensions;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "Koltuk")]
    public class Koltuk : InpcBase
    {
        private bool görünür;

        private bool koltukDolu;

        private bool koltukEtkin;

        private int koltukTipiId;

        private ObservableCollection<Musteri> müşteri = new();

        private int no;

        private KoltukTipi seçiliKoltukTipi;

        [XmlAttribute(AttributeName = "Görünür")]
        public bool Görünür
        {
            get => görünür;

            set
            {
                if (görünür != value)
                {
                    görünür = value;
                    OnPropertyChanged(nameof(Görünür));
                }
            }
        }

        [XmlIgnore]
        public bool KoltukDolu
        {
            get => koltukDolu;

            set
            {
                if (koltukDolu != value)
                {
                    koltukDolu = value;
                    OnPropertyChanged(nameof(KoltukDolu));
                }
            }
        }

        [XmlAttribute(AttributeName = "KoltukEtkin")]
        public bool KoltukEtkin
        {
            get => koltukEtkin;

            set
            {
                if (koltukEtkin != value)
                {
                    koltukEtkin = value;
                    OnPropertyChanged(nameof(KoltukEtkin));
                }
            }
        }

        [XmlAttribute(AttributeName = "KoltukTipiId")]
        public int KoltukTipiId
        {
            get => koltukTipiId;

            set
            {
                if (koltukTipiId != value)
                {
                    koltukTipiId = value;
                    OnPropertyChanged(nameof(KoltukTipiId));
                }
            }
        }

        [XmlElement(ElementName = "Müşteri")]
        public ObservableCollection<Musteri> Müşteri
        {
            get => müşteri;

            set
            {
                if (müşteri != value)
                {
                    müşteri = value;
                    OnPropertyChanged(nameof(Müşteri));
                }
            }
        }

        [XmlAttribute(AttributeName = "No")]
        public int No
        {
            get => no;

            set
            {
                if (no != value)
                {
                    no = value;
                    OnPropertyChanged(nameof(No));
                }
            }
        }

        [XmlIgnore]
        public KoltukTipi SeçiliKoltukTipi
        {
            get => seçiliKoltukTipi;

            set
            {
                if (seçiliKoltukTipi != value)
                {
                    seçiliKoltukTipi = value;
                    OnPropertyChanged(nameof(SeçiliKoltukTipi));
                }
            }
        }
    }
}