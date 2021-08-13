using Extensions;
using System;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "KoltukTipi")]
    public class KoltukTipi : InpcBase
    {
        private int ıd;

        private string koltukAçıklama;

        private double koltukFiyatı = 1;

        private string koltukRenk = typeof(Colors).GetProperties().Select(z => z.Name.Replace("System.Windows.Media.Colors ", "")).Where(z => z is not "Black" and not "White" and not "Transparent").OrderBy(_ => Guid.NewGuid()).Take(1).First();

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