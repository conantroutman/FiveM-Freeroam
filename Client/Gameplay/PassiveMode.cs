using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Client.Gameplay
{
    class PassiveMode
    {
        private static bool isPassiveModeActive = false;
        private static PlayerList players;

        public static void Enable()
        {
            if (!isPassiveModeActive)
            {
                API.NetworkSetPlayerIsPassive(true);
                HUD.GamerTags.SetPassiveMode(true);

                players = new PlayerList();
                foreach(Player player in players)
                {
                    API.SetPedCanBeTargettedByPlayer(player.Character.Handle, Game.Player.Handle, false);
                    API.SetPedCanBeTargettedByPlayer(Game.Player.Character.Handle, player.Handle, false);
                }
            }
        }
    }
}
