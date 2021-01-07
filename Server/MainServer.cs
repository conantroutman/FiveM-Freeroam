using CitizenFX.Core;
using System;
using System.Dynamic;

namespace Server
{
    public class MainServer : BaseScript
    {
        public MainServer()
        {
            Debug.WriteLine("Started the server.");

            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);

            EventHandlers["baseevents:onPlayerDied"] += new Action<Player>(OnPlayerDied);
            EventHandlers["baseevents:onPlayerKilled"] += new Action<Player, int, dynamic>(OnPlayerKilled);

        }

        private async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            // mandatory wait!
            await Delay(0);

            var licenseIdentifier = player.Identifiers["license"];

            Debug.WriteLine($"A player with the name {playerName} (Identifier: [{licenseIdentifier}]) is connecting to the server.");

            deferrals.update($"Hello {playerName}, your license [{licenseIdentifier}] is being checked");

            // Checking ban list
            // - assuming you have a function called IsBanned of type Task<bool>
            // - normally you'd do a database query here, which might take some time
            /*if (await IsBanned(licenseIdentifier))
            {
                deferrals.done($"You have been kicked (Reason: [Banned])! Please contact the server administration (Identifier: [{licenseIdentifier}]).");
            }*/

            deferrals.done();

            TriggerClientEvent("playerJoined", $"~HUD_COLOUR_NET_PLAYER{player.Handle}~<C>{player.Name}</C>~w~ joined.");
        }

        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            Debug.WriteLine($"Player {player.Name} dropped (Reason: {reason}).");

            TriggerClientEvent("playerLeft", $"~HUD_COLOUR_NET_PLAYER{player.Handle}~<C>{player.Name}</C>~w~ left.");
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

        private void OnPlayerKilled([FromSource] Player victim, int killerId, dynamic deathData)
        {
            Debug.WriteLine();
            TriggerClientEvent("playerKilled", victim.Handle);
        }
    }
}
