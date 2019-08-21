using System.Linq;
using System.Reflection;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Harmony;

namespace DaeEsyaKaraListeleyici
{
	public class EşyaKaraListeleyici : RocketPlugin<EşyaKaraListeleyiciYapılandırma>
    {
        public static EşyaKaraListeleyici Örnek { get; private set; }
        private HarmonyInstance _harmony;

        protected override void Load()
        {
            Örnek = this;

            _harmony = HarmonyInstance.Create("dae.esyakaralisteleyici");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

			ItemManager.onTakeItemRequested += EşyaAlınmasıİstendiğinde;
            UnturnedPlayerEvents.OnPlayerInventoryAdded += EşyaAlındığında;
        }

        protected override void Unload()
		{
            Örnek = null;

            _harmony.UnpatchAll("dae.esyakaralisteleyici");
            _harmony = null;

			ItemManager.onTakeItemRequested -= EşyaAlınmasıİstendiğinde;
		    UnturnedPlayerEvents.OnPlayerInventoryAdded -= EşyaAlındığında;
        }

        private void EşyaAlınmasıİstendiğinde(Player eşyaAlanOyuncu, byte x, byte y, uint örnekIdsi, byte toX, byte toY, byte toRot, byte toPage, ItemData eşyaVerisi, ref bool eşyayıAlabilir)
		{
		    var oyuncu = UnturnedPlayer.FromPlayer(eşyaAlanOyuncu);
            if (oyuncu.IsAdmin)
            {
                return;
            }

		    var eşya = ItemManager.regions[x, y].items.FirstOrDefault(e => e.instanceID == örnekIdsi);
            if (eşya?.item == null || !Configuration.Instance.KaraListeler.SelectMany(k => k.Eşyalar).Select(e => e.Id).Contains(eşya.item.id)
                                   || Configuration.Instance.KaraListeler.Any(k => oyuncu.HasPermission($"dae.esyakaralisteleyici.{k.KaraListeİsmi}")
                                                                                   && k.Eşyalar.Any(e => e.Id == eşya.item.id)))
            {
                return;
            }

		    eşyayıAlabilir = false;
		}

	    private void EşyaAlındığında(UnturnedPlayer oyuncu, InventoryGroup sayfa, byte sıra, ItemJar eşyaKutusu)
	    {
            if (oyuncu.IsAdmin)
            {
                return;
            }

            if (eşyaKutusu?.item == null || !Configuration.Instance.KaraListeler.SelectMany(k => k.Eşyalar).Select(e => e.Id).Contains(eşyaKutusu.item.id)
                                         || Configuration.Instance.KaraListeler.Any(k => oyuncu.HasPermission($"dae.esyakaralisteleyici.{k.KaraListeİsmi}")
                                                                                         && k.Eşyalar.Any(e => e.Id == eşyaKutusu.item.id)))
            {
                return;
            }
			
	        oyuncu.Inventory.askDropItem(oyuncu.CSteamID, (byte)sayfa, eşyaKutusu.x, eşyaKutusu.y);
	    }
	}
}