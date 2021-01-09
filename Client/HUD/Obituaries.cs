using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace Client.HUD
{
    class Obituaries : BaseScript
    {
        enum DeathCauses
        {
            Generic,
            MeleeSharp,
            MeleeBlunt,
            AssaultRifle,
            SMG,
            Explosive,
            Rocket,
            Minigun,
            Shotgun,
            Sniper,
            Pistol,
            MachineGun,
            Vehicle,
            Helicopter
        }

        public Obituaries()
        {
            //Constructor
            EventHandlers["playerDied"] += new Action<int>(OnPlayerDied);
            EventHandlers["playerKilled"] += new Action<int, int, string>(OnPlayerKilled);
        }

        private void OnPlayerDied(int victimId)
        {
            string victimName = GetPlayerFromServerId(victimId) == Game.Player.Handle ? "You" : $"~HUD_COLOUR_NET_PLAYER{victimId}~<C>{GetPlayerName(GetPlayerFromServerId(victimId))}</C>~w~";
            Screen.ShowNotification($"{victimName} died.");
        }

        private void OnPlayerKilled(int victimId, int weaponHash, string killTerm)
        {
            Player victim = Players[victimId];
            // Gets player server ID based on which entity killed the victim
            int killerId = GetKillerId(victim);
            if (killerId != 0)
            {
                Player killer = Players[killerId];
                String killerString = killer.Handle == Game.Player.Handle ? "You" : $"~HUD_COLOUR_NET_PLAYER{killerId}~<C>{killer.Name}</C>~w~";
                String victimString = victim.Handle == Game.Player.Handle ? "you" : $"~HUD_COLOUR_NET_PLAYER{victimId}~<C>{victim.Name}</C>~w~";
                String killString = weaponHash == GetHashKey("weapon_pistol") ? "pistoled" : "killed";
                String obituary = DoesKillTermNeedFormatting(killTerm) && victim.Handle == Game.Player.Handle ? $"{killerString} {FormatKillTerm(killTerm)}." : $"{killerString} {killTerm} {victimString}.";
                Screen.ShowNotification(obituary);
            } else
            {
                OnPlayerDied(victimId);
            }
            
        }

        private String GetDeathMessage()
        {
            return "killed";
        }

        private int GetKillerId(Player victim)
        {
            int killer = GetPedSourceOfDeath(victim.Character.Handle);
            if (killer != 0)
            {
                int killerId = GetPlayerServerId(NetworkGetPlayerIndexFromPed(killer));
                return killerId;
            } else
            {
                return 0;
            }
        }

        private bool DoesKillTermNeedFormatting(string term)
        {
            bool needFormatting = false;
            switch (term)
            {
                case "beat down":
                case "cut up":
                case "crossed out":
                case "cut down":
                case "blew away":
                case "opened up":
                    needFormatting = true;
                    break;
                default:
                    needFormatting = false;
                    break;
            }
            return needFormatting;
        }

        private string FormatKillTerm(string term)
        {
            string[] words = term.Split(' ');
            return $"{words[0]} you {words[1]}";
        }
    }
}
