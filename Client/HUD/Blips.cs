using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace Client.HUD
{
    class Blips
    {
        private static int blip;
        private static int vehicle;
        private static dynamic activePlayers;
        private static PlayerList players;

        public async static void Update()
        {
            foreach(Player player in players)
            {
                blip = API.GetBlipFromEntity(player.Character.Handle);
                if(player.Character.IsDead)
                {
                    API.SetBlipSprite(blip, (int)BlipSprite.Dead);
                }
            }
        }

        public static void Create()
        {
            players = new PlayerList();
            Debug.WriteLine("We are in the Create method.");
            foreach(Player player in players)
            {
                Debug.WriteLine(player.ServerId.ToString());
                if (!API.DoesBlipExist(blip))
                {
                    Debug.WriteLine("Creating blip");
                    blip = API.AddBlipForEntity(player.Character.Handle);
                    API.SetBlipSprite(blip, 1);
                    API.ShowHeadingIndicatorOnBlip(blip, true);
                    API.SetBlipColour(blip, Players.Colors.GetOtherPlayersColor(player.ServerId) - 22);
                    API.SetBlipNameToPlayerName(blip, player.Handle);
                    API.SetBlipCategory(blip, 7);
                }
            }
        }
    }
}
