using System.Xml.Serialization;

namespace DaeEsyaKaraListeleyici.Modeller
{
    public class Eşya
    {
        [XmlAttribute]
        public ushort Id { get; set; }

        public Eşya()
        {
        }

        public Eşya(ushort id)
        {
            Id = id;
        }
    }
}