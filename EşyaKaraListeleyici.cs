using System.Linq;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace DaeEsyaKaraListeleyici
{
    public class EşyaKaraListeleyici : RocketPlugin<EşyaKaraListeleyiciYapılandırma>
    {
        protected override void Load()
        {
            ItemManager.onTakeItemRequested += EşyaAlınmasıİstendiğinde;
            UnturnedPlayerEvents.OnPlayerInventoryAdded += EşyaAlındığında;
            PlayerCrafting.onCraftBlueprintRequested += EşyaOluşturmasıİstendiğinde;
        }

        protected override void Unload()
        {
            ItemManager.onTakeItemRequested -= EşyaAlınmasıİstendiğinde;
            UnturnedPlayerEvents.OnPlayerInventoryAdded -= EşyaAlındığında;
            PlayerCrafting.onCraftBlueprintRequested -= EşyaOluşturmasıİstendiğinde;
        }

        private void EşyaAlınmasıİstendiğinde(Player eşyaAlanOyuncu, byte x, byte y, uint örnekIdsi, byte hedefX, byte hedefY, byte hedefAçı, byte hedefSayfa, ItemData eşyaVerisi, ref bool eşyayıAlabilir)
        {
            var oyuncu = UnturnedPlayer.FromPlayer(eşyaAlanOyuncu);
            if (oyuncu.IsAdmin)
            {
                return;
            }

            var eşya = ItemManager.regions[x, y].items.FirstOrDefault(e => e.instanceID == örnekIdsi);
            if (eşya?.item != null && EşyaKaraListede(eşya.item.id) && !EşyayıAlabilir(oyuncu, eşya.item.id))
            {
                eşyayıAlabilir = false;
            }
        }

        private void EşyaAlındığında(UnturnedPlayer oyuncu, InventoryGroup sayfa, byte sıra, ItemJar eşyaKutusu)
        {
            if (oyuncu.IsAdmin)
            {
                return;
            }

            if (eşyaKutusu?.item != null && EşyaKaraListede(eşyaKutusu.item.id) && !EşyayıAlabilir(oyuncu, eşyaKutusu.item.id))
            {
                oyuncu.Inventory.askDropItem(oyuncu.CSteamID, (byte)sayfa, eşyaKutusu.x, eşyaKutusu.y);
            }
        }

        private void EşyaOluşturmasıİstendiğinde(PlayerCrafting örnek, ref ushort id, ref byte sıra, ref bool eşyayıOluşturabilir)
        {
            var oyuncu = UnturnedPlayer.FromPlayer(örnek.player);
            if (oyuncu.IsAdmin)
            {
                return;
            }

            if (EşyaKaraListede(id) && !EşyayıOluşturabilir(oyuncu, id))
            {
                eşyayıOluşturabilir = false;
            }
        }

        private bool EşyaKaraListede(ushort id) => Configuration.Instance.KaraListeler.Any(k => k.Eşyalar.Any(e => e.Id == id));

        private bool EşyayıAlabilir(IRocketPlayer oyuncu, ushort id) => Configuration.Instance.KaraListeler.Any(k => oyuncu.HasPermission($"dae.esyakaralisteleyici.{k.KaraListeİsmi}")
                                                                                                                     && k.Eşyalar.Any(e => e.Id == id));

        private bool EşyayıOluşturabilir(IRocketPlayer oyuncu, ushort id) => Configuration.Instance.KaraListeler.Any(k => oyuncu.HasPermission($"dae.esyakaralisteleyici.{k.KaraListeİsmi}.o")
                                                                                                                          && k.Eşyalar.Any(e => e.Id == id));
    }
}