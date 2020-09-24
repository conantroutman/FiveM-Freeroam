using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Client.Players
{
    internal class Colors
    {
        private static int[] playerColors = { 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };

        public async static void Loop()
        {
            await BaseScript.Delay(500);
            API.ResetPlayerStamina(Game.Player.Handle);
        }
        public static void Setup()
        {
            API.ReplaceHudColour(116, GetColor());
        }

        public static int GetColor()
        {
            return playerColors[Game.Player.ServerId - 1];
        }

        public static int GetOtherPlayersColor(int serverId)
        {
            return playerColors[serverId - 1];
        }
    }
}
