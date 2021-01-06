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
        }

        // Possible death messages
        public readonly struct DeathMessages
        {
            
        }

        private void OnPlayerDied(int victimId)
        {
            Debug.WriteLine(victimId.ToString());
            string victimName = API.GetPlayerFromServerId(victimId) == Game.Player.Handle ? "You" : $"<C>{API.GetPlayerName(API.GetPlayerFromServerId(victimId))}</C>";
            Screen.ShowNotification($"{victimName} died.");
        }

        private void OnPlayerKilled([FromSource] Player victim, int killerId)
        {
            Debug.WriteLine("Someone died");
            Player killer = Players[killerId];
            ShowObituary(victim, killer);
        }

        private void Test()
        {
            Screen.ShowNotification($"Someone got fucked up.");
        }

        private void ShowObituary(Player victim, Player killer = null)
        {

            String message = GetDeathMessage();
            String killerString = killer.Handle == Game.Player.Handle ? "You" : $"<C>{killer.Name}</C>";
            String victimString = victim.Handle == Game.Player.Handle ? "you" : $"<C>{victim.Name}</C>";
            Screen.ShowNotification($"{killerString} {message} {victimString}.");
            Debug.WriteLine($"{killerString} {message} {victimString}.");
        }

        private String GetDeathMessage()
        {
            return "killed";
        }

        [EventHandler("onResourceStop")]
        private void OnResourceStop(string name)
        {
            if (name == API.GetCurrentResourceName())
            {
                // Do stuff when the resource stops
            }
        }

    }
}
