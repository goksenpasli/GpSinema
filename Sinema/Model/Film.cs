using Extensions;
using System;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "Film")]
    public class Film : InpcBase
    {
        private string adı;

        private DateTime filmSaati;

        private int filmTipi = -1;

        private int ıd;

        private string oyuncular;

        private string renk = typeof(Colors).GetProperties().Select(z => z.Name.Replace("System.Windows.Media.Colors ", "")).Where(z => z is not "Black" and not "White" and not "Transparent").OrderBy(_ => Guid.NewGuid()).Take(1).First();

        private string resimYolu;

        private double süre;

        private string videoYolu;

        private string yönetmen;

        [XmlAttribute(AttributeName = "Adı")]
        public string Adı
        {
            get => adı;

            set
            {
                if (adı != value)
                {
                    adı = value;
                    OnPropertyChanged(nameof(Adı));
                }
            }
        }

        [XmlAttribute(AttributeName = "FilmSaati")]
        public DateTime FilmSaati
        {
            get => filmSaati;

            set
            {
                if (filmSaati != value)
                {
                    filmSaati = value;
                    OnPropertyChanged(nameof(FilmSaati));
                }
            }
        }

        [XmlAttribute(AttributeName = "FilmTipi")]
        public int FilmTipi
        {
            get => filmTipi;

            set
            {
                if (filmTipi != value)
                {
                    filmTipi = value;
                    OnPropertyChanged(nameof(FilmTipi));
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

        [XmlAttribute(AttributeName = "Oyuncular")]
        public string Oyuncular
        {
            get => oyuncular;

            set
            {
                if (oyuncular != value)
                {
                    oyuncular = value;
                    OnPropertyChanged(nameof(Oyuncular));
                }
            }
        }

        [XmlAttribute(AttributeName = "Renk")]
        public string Renk
        {
            get => renk;

            set
            {
                if (renk != value)
                {
                    renk = value;
                    OnPropertyChanged(nameof(Renk));
                }
            }
        }

        [XmlAttribute(AttributeName = "ResimYolu")]
        public string ResimYolu
        {
            get => resimYolu;

            set
            {
                if (resimYolu != value)
                {
                    resimYolu = value;
                    OnPropertyChanged(nameof(ResimYolu));
                }
            }
        }

        [XmlAttribute(AttributeName = "Süre")]
        public double Süre
        {
            get => süre;

            set
            {
                if (süre != value)
                {
                    süre = value;
                    OnPropertyChanged(nameof(Süre));
                }
            }
        }

        [XmlAttribute(AttributeName = "VideoYolu")]
        public string VideoYolu
        {
            get => videoYolu;

            set
            {
                if (videoYolu != value)
                {
                    videoYolu = value;
                    OnPropertyChanged(nameof(VideoYolu));
                }
            }
        }

        [XmlAttribute(AttributeName = "Yönetmen")]
        public string Yönetmen
        {
            get => yönetmen;

            set
            {
                if (yönetmen != value)
                {
                    yönetmen = value;
                    OnPropertyChanged(nameof(Yönetmen));
                }
            }
        }
    }
}