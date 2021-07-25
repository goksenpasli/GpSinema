using Extensions;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "Urunler")]
    public class Urunler : InpcBase
    {
        private ObservableCollection<Urun> urun = new();

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
    }
}