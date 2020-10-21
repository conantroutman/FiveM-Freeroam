using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Client.HUD
{
    class GamerTags
    {
        private static PlayerList players;
        private static int gamerTag;
        private static bool isPassiveModeActive;

        public static void Create()
        {
            players = new PlayerList();
            foreach(Player player in players)
            {
                if(player != Game.Player && player.Character.IsAlive)
                {
                    gamerTag = API.CreateFakeMpGamerTag(player.Character.Handle, player.Name, false, false, "", 0);
                    API.SetMpGamerTagColour(gamerTag, 0, Players.Colors.GetColor(player));
                    API.SetMpGamerTagColour(gamerTag, 6, Players.Colors.GetColor(player));
                    API.SetMpGamerTagColour(gamerTag, 7, Players.Colors.GetColor(player));
                    API.SetMpGamerTagColour(gamerTag, 8, Players.Colors.GetColor(player));
                }
            }
        }

        public async static void Update()
        {
            foreach (Player player in players)
            {
                if (player != Game.Player)
                {
                    // Did the player respawn?
                    if(!player.Character.IsDead && !API.IsValidMpGamerTagMovie(gamerTag))
                    {
                        Create();
                    }
                    else if (player.Character.IsDead)
                    {
                        API.RemoveMpGamerTag(gamerTag);
                    }

                    // Wanted
                    if (player.WantedLevel > 0)
                    {
                        API.SetMpGamerTagVisibility(gamerTag, 7, true);
                        API.SetMpGamerTagWantedLevel(gamerTag, player.WantedLevel);
                    }
                    else
                    {
                        API.SetMpGamerTagVisibility(gamerTag, 7, false);
                    }

                    // Driving
                    if (player.Character.IsInVehicle())
                    {
                        API.SetMpGamerTagVisibility(gamerTag, 8, true);
                    }
                    else
                    {
                        API.SetMpGamerTagVisibility(gamerTag, 8, false);
                    }

                    // Passive mode
                    if (isPassiveModeActive)
                    {
                        API.SetMpGamerTagVisibility(gamerTag, 6, true);
                    }
                    else
                    {
                        API.SetMpGamerTagVisibility(gamerTag, 6, false);
                    }
                }
            }
        }

        public static void SetPassiveMode(bool toggle)
        {
            isPassiveModeActive = toggle;
        }
        public static void AddGamerTagComponent(int gamerTag, int component)
        {
            if(gamerTag != null)
            {
                API.SetMpGamerTagVisibility(gamerTag, component, true);
            }
        }

        public static void RemoveGamerTagComponent(int gamerTag, int component)
        {
            if (gamerTag != null)
            {
                API.SetMpGamerTagVisibility(gamerTag, component, false);
            }
        }
    }
}
