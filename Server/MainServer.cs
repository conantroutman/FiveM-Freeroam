using CitizenFX.Core;
using System;

namespace Server
{
    public class MainServer : BaseScript
    {
        private PlayerList playerList;
        public MainServer()
        {
            Debug.WriteLine("Started the server.");
            playerList = new PlayerList();
            foreach(Player player in playerList)
            {
                Debug.WriteLine($"{player.Name} ID: {player.Handle}");
            }
        }

        [EventHandler("playerConnecting")]
        private void playerConnecting([FromSource] Player connectedPlayer)
        {
            Debug.WriteLine($"Player {connectedPlayer.Name} connected");

            playerList = new PlayerList();
            foreach(Player player in playerList)
            {
                player.TriggerEvent("newPlayerConnected", connectedPlayer);
            }
        }

        [EventHandler("playerDropped")]
        private void playerDropped([FromSource] Player droppedPlayer, string reason)
        {
            Debug.WriteLine($"Player {droppedPlayer.Name} disconnected");
            playerList = new PlayerList();
            foreach (Player player in playerList)
            {
                player.TriggerEvent("playerDisconnected", droppedPlayer);
            }
        }
    }
}
