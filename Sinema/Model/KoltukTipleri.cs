using Extensions;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [XmlRoot(ElementName = "KoltukTipleri")]
    public class KoltukTipleri : InpcBase
    {
        private ObservableCollection<KoltukTipi> koltukTipi = new();

        [XmlElement(ElementName = "KoltukTipi")]
        public ObservableCollection<KoltukTipi> KoltukTipi
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
    }
}