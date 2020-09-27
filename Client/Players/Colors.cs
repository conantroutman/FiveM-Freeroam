using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Linq;

namespace Client.Players
{
    internal class Colors
    {
        private static PlayerList playerlist;

        private static int[] playerColors = { 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };

        public async static void Loop()
        {
            await BaseScript.Delay(500);
            API.ResetPlayerStamina(Game.Player.Handle);
        }
        public static void Setup()
        {
            playerlist = new PlayerList();
            API.ReplaceHudColour(116, GetColor(Game.Player));
        }

        public static int GetColor(Player player)
        {
            return playerColors[playerlist.ToList().IndexOf(player)];
        }

        public static int GetOtherPlayersColor(int serverId)
        {
            return playerColors[serverId - 1];
        }
    }
}
