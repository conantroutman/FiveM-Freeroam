using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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
            EventHandlers["playerKilled"] += new Action<int>(OnPlayerKilled);
        }

        // Possible death messages
        public readonly struct DeathMessages
        {
            
        }

        private void OnPlayerDied(int victimId)
        {
            string victimName = API.GetPlayerFromServerId(victimId) == Game.Player.Handle ? "You" : $"~HUD_COLOUR_NET_PLAYER{victimId}~<C>{API.GetPlayerName(API.GetPlayerFromServerId(victimId))}</C>~w~";
            Screen.ShowNotification($"{victimName} died.");
        }

        private void OnPlayerKilled(int victimId)
        {
            Player victim = Players[victimId];
            // Gets player server ID based on which entity killed the victim
            Debug.WriteLine($"{API.GetPedSourceOfDeath(victim.Character.Handle)}");
            int killerId = GetKillerId(victim);
            if (killerId != 0)
            {
                Player killer = Players[killerId];
                Debug.WriteLine($"{killer.Name} killed {victim.Name}.");
                String killerString = killer.Handle == Game.Player.Handle ? "You" : $"~HUD_COLOUR_NET_PLAYER{killerId}~<C>{killer.Name}</C>~w~";
                String victimString = victim.Handle == Game.Player.Handle ? "you" : $"~HUD_COLOUR_NET_PLAYER{victimId}~<C>{victim.Name}</C>~w~";
                Screen.ShowNotification($"{killerString} killed {victimString}.");
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
            int killer = API.GetPedSourceOfDeath(victim.Character.Handle);
            if (killer != 0)
            {
                int killerId = API.GetPlayerServerId(API.NetworkGetPlayerIndexFromPed(killer));
                return killerId;
            } else
            {
                Debug.WriteLine(API.GetPedCauseOfDeath(victim.Character.Handle).ToString());
                return 0;
            }
        }
    }
}
