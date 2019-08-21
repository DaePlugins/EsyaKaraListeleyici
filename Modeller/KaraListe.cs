using System.Collections.Generic;
using System.Xml.Serialization;

namespace DaeEsyaKaraListeleyici.Modeller
{
    public class KaraListe
    {
        [XmlAttribute]
        public string KaraListeİsmi { get; set; }

        public List<Eşya> Eşyalar { get; set; }

        public KaraListe()
        {
        }

        public KaraListe(string karaListeİsmi, List<Eşya> eşyalar)
        {
            KaraListeİsmi = karaListeİsmi;

            Eşyalar = eşyalar;
        }
    }
}