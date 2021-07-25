using Extensions;
using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "Siparis")]
    public class Siparis : InpcBase
    {
        private DateTime tarih;

        private double toplamTutar;

        private ObservableCollection<Urun> urun = new();

        private int urunAdet = 1;

        [XmlAttribute(AttributeName = "Tarih")]
        public DateTime Tarih
        {
            get => tarih;

            set
            {
                if (tarih != value)
                {
                    tarih = value;
                    OnPropertyChanged(nameof(Tarih));
                }
            }
        }

        [XmlIgnore]
        public double ToplamTutar
        {
            get => toplamTutar;

            set
            {
                if (toplamTutar != value)
                {
                    toplamTutar = value;
                    OnPropertyChanged(nameof(ToplamTutar));
                }
            }
        }

        [XmlElement(ElementName = "Urun")]
        public ObservableCollection<Urun> Urun
        {
            get => urun;

            set
            {
                if (urun != value)
                {
                    urun = value;
                    OnPropertyChanged(nameof(Urun));
                }
            }
        }

        [XmlIgnore]
        public int UrunAdet
        {
            get => urunAdet;

            set
            {
                if (urunAdet != value)
                {
                    urunAdet = value;
                    OnPropertyChanged(nameof(UrunAdet));
                }
            }
        }
    }
}