using System.Collections.Generic;
using DaeEsyaKaraListeleyici.Modeller;
using Rocket.API;

namespace DaeEsyaKaraListeleyici
{
    public class EşyaKaraListeleyiciYapılandırma : IRocketPluginConfiguration
    {
        public List<KaraListe> KaraListeler { get; set; } = new List<KaraListe>();

        public void LoadDefaults()
        {
            KaraListeler = new List<KaraListe>
            {
                new KaraListe
                (
                    "patlayici",
                    new List<Eşya>
                    {
                        new Eşya(519),
                        new Eşya(254),
                        new Eşya(1100)
                    }
                ),
                new KaraListe
                (
                    "silah",
                    new List<Eşya>
                    {
                        new Eşya(307),
                        new Eşya(308),
                        new Eşya(309),
                        new Eşya(310),
                        new Eşya(363),
                        new Eşya(116)
                    }
                )
            };
        }
    }
}