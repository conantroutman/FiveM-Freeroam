using CitizenFX.Core;
using System;

namespace Server
{
    public class MainServer : BaseScript
    {
        public MainServer()
        {
            Debug.WriteLine("Started the server.");

            EventHandlers["baseevents:onPlayerDied"] += new Action<Player>(OnPlayerDied);
        }

        private void OnPlayerDied([FromSource] Player victim)
        {
            Debug.WriteLine($"{victim.Name} died.");

            PlayerList playerList = new PlayerList();
            foreach (Player player in playerList)
            {
                player.TriggerEvent("playerDied", victim.Handle);
            }
        }
    }
}
