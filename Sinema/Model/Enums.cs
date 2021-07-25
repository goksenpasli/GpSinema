using System;
using System.Xml.Serialization;

namespace Sinema.Model
{
    [Serializable]
    public enum KoltukÇeşidi
    {
        [XmlEnum(Name = "İndirimli")]
        İndirimli = 0,

        [XmlEnum(Name = "Lüks")]
        Lüks = 1,
    }

    [Serializable]
    public enum KoltukDurum
    {
        [XmlEnum(Name = "Boş")]
        Boş = 0,

        [XmlEnum(Name = "Dolu")]
        Dolu = 1,
    }
}