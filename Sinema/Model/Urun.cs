using Extensions;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "Urun")]
    public class Urun : InpcBase
    {
        private int adet = 1;

        private double birimFiyat = 1;

        private double toplamFiyat;

        private string ürünAdi;

        [XmlAttribute(AttributeName = "Adet")]
        public int Adet
        {
            get => adet;

            set
            {
                if (adet != value)
                {
                    adet = value;
                    OnPropertyChanged(nameof(Adet));
                    OnPropertyChanged(nameof(ToplamFiyat));
                }
            }
        }

        [XmlAttribute(AttributeName = "BirimFiyat")]
        public double BirimFiyat
        {
            get => birimFiyat;

            set
            {
                if (birimFiyat != value)
                {
                    birimFiyat = value;
                    OnPropertyChanged(nameof(BirimFiyat));
                    OnPropertyChanged(nameof(ToplamFiyat));
                }
            }
        }

        [XmlIgnore]
        public double ToplamFiyat
        {
            get
            {
                toplamFiyat = BirimFiyat * Adet;
                return toplamFiyat;
            }

            set
            {
                if (toplamFiyat != value)
                {
                    toplamFiyat = value;
                    OnPropertyChanged(nameof(ToplamFiyat));
                }
            }
        }

        [XmlAttribute(AttributeName = "ÜrünAdi")]
        public string ÜrünAdi
        {
            get => ürünAdi;

            set
            {
                if (ürünAdi != value)
                {
                    ürünAdi = value;
                    OnPropertyChanged(nameof(ÜrünAdi));
                }
            }
        }
    }
}