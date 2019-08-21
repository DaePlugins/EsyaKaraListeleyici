using System.Linq;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using Harmony;

namespace DaeEsyaKaraListeleyici
{
    [HarmonyPatch(typeof(PlayerCrafting))]
    [HarmonyPatch("askCraft")]
    internal class YamaOluşturma
    {
        [HarmonyPrefix]
        private static bool EşyaOluşturulmadanÖnce(CSteamID steamID, ushort id)
        {
            var oyuncu = UnturnedPlayer.FromCSteamID(steamID);
            if (oyuncu.IsAdmin)
            {
                return true;
            }

            if (!EşyaKaraListeleyici.Örnek.Configuration.Instance.KaraListeler.SelectMany(k => k.Eşyalar).Select(e => e.Id).Contains(id)
                || EşyaKaraListeleyici.Örnek.Configuration.Instance.KaraListeler.Any(k => oyuncu.HasPermission($"dae.esyakaralisteleyici.{k.KaraListeİsmi}")
                                                                                          && k.Eşyalar.Any(e => e.Id == id)))
            {
                return true;
            }

            return false;
        }
    }
}