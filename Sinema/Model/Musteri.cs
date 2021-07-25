using Extensions;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "Musteri")]
    public class Musteri : InpcBase
    {
        private string ad;

        private double biletfiyat;

        private int filmId;

        private int ıd;

        private Musteri seçiliMusteri;

        private Siparis siparis = new();

        private string soyad;

        private int yas = 18;

        [XmlAttribute(AttributeName = "Ad")]
        public string Ad
        {
            get => ad;

            set
            {
                if (ad != value)
                {
                    ad = value;
                    OnPropertyChanged(nameof(Ad));
                }
            }
        }

        [XmlAttribute(AttributeName = "BiletFiyat")]
        public double BiletFiyat
        {
            get => biletfiyat;

            set
            {
                if (biletfiyat != value)
                {
                    biletfiyat = value;
                    OnPropertyChanged(nameof(BiletFiyat));
                }
            }
        }

        [XmlAttribute(AttributeName = "FilmId")]
        public int FilmId
        {
            get => filmId;

            set
            {
                if (filmId != value)
                {
                    filmId = value;
                    OnPropertyChanged(nameof(FilmId));
                }
            }
        }

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

        [XmlIgnore]
        public Musteri SeçiliMusteri
        {
            get => seçiliMusteri;

            set
            {
                if (seçiliMusteri != value)
                {
                    seçiliMusteri = value;
                    OnPropertyChanged(nameof(SeçiliMusteri));
                }
            }
        }

        [XmlElement(ElementName = "Siparis")]
        public Siparis Siparis
        {
            get => siparis;

            set
            {
                if (siparis != value)
                {
                    siparis = value;
                    OnPropertyChanged(nameof(Siparis));
                }
            }
        }

        [XmlAttribute(AttributeName = "Soyad")]
        public string Soyad
        {
            get => soyad;

            set
            {
                if (soyad != value)
                {
                    soyad = value;
                    OnPropertyChanged(nameof(Soyad));
                }
            }
        }

        [XmlAttribute(AttributeName = "Yas")]
        public int Yas
        {
            get => yas;

            set
            {
                if (yas != value)
                {
                    yas = value;
                    OnPropertyChanged(nameof(Yas));
                }
            }
        }
    }
}