using Extensions;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "KoltukTipi")]
    public class KoltukTipi : InpcBase
    {
        private int ıd;

        private string koltukAçıklama;

        private double koltukFiyatı = 1;

        private string koltukRenk;

        [XmlAttribute(AttributeName = "Id")]
        public int Id
        {
            get => ıd;

            set
            {
                if (ıd != value)
                {
                    ıd = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        [XmlAttribute(AttributeName = "KoltukAçıklama")]
        public string KoltukAçıklama
        {
            get => koltukAçıklama;

            set
            {
                if (koltukAçıklama != value)
                {
                    koltukAçıklama = value;
                    OnPropertyChanged(nameof(KoltukAçıklama));
                }
            }
        }

        [XmlAttribute(AttributeName = "KoltukFiyatı")]
        public double KoltukFiyatı
        {
            get => koltukFiyatı;

            set
            {
                if (koltukFiyatı != value)
                {
                    koltukFiyatı = value;
                    OnPropertyChanged(nameof(KoltukFiyatı));
                }
            }
        }

        [XmlAttribute(AttributeName = "KoltukRenk")]
        public string KoltukRenk
        {
            get => koltukRenk;

            set
            {
                if (koltukRenk != value)
                {
                    koltukRenk = value;
                    OnPropertyChanged(nameof(KoltukRenk));
                }
            }
        }
    }
}