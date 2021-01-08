using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Client.HUD
{
    // https://docs.fivem.net/docs/game-references/gamer-tags/
    class GamerTags
    {
        private static PlayerList players;
        private static int gamerTag;
        private static bool isPassiveModeActive;
        private static bool isHealthBarVisible = false;

        private static float distance = 800f;

        public GamerTags()
        {
            Create();
        }

        public static void Create()
        {
            players = new PlayerList();
            foreach(Player player in players)
            {
                if(player != Game.Player && player.Character.IsAlive)
                {
                    gamerTag = API.CreateFakeMpGamerTag(player.Character.Handle, player.Name, false, false, "", 0);
                    API.SetMpGamerTagColour(gamerTag, 0, 27 + player.ServerId);

                    API.SetMpGamerTagAlpha(gamerTag, 2, 255);
                    API.SetMpGamerTagColour(gamerTag, 2, 27 + player.ServerId);

                    API.SetMpGamerTagColour(gamerTag, 6, 27 + player.ServerId);
                    API.SetMpGamerTagColour(gamerTag, 7, 27 + player.ServerId);
                    API.SetMpGamerTagColour(gamerTag, 8, 27 + player.ServerId);
                    //API.SetMpGamerHealthBarDisplay(gamerTag, true);
                    API.SetMpGamerTagHealthBarColor(gamerTag, 27 + player.ServerId);
                }
            }
        }

        public async static void Update()
        {
            // Only display for the player currently aimed at?
            foreach (Player player in players)
            {
                if (player != Game.Player)
                {
                    // Did the player respawn?
                    if(!player.Character.IsDead && !API.IsValidMpGamerTagMovie(gamerTag))
                    {
                        Create();
                    } else if (player.Character.IsDead)
                    {
                        API.RemoveMpGamerTag(gamerTag);
                    }


                    if ((IsOtherPlayerWithinDistance(player) || IsPlayerAimingAtOtherPlayer(player)) && IsOtherPlayerVisible(player))
                    {
                        API.SetMpGamerTagVisibility(gamerTag, 0, true);

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

                        // Is player aiming at 
                        if (IsPlayerAimingAtOtherPlayer(player))
                        {
                            // Make healthbar visible
                            API.SetMpGamerTagVisibility(gamerTag, 2, true);
                        } else
                        {
                            API.SetMpGamerTagVisibility(gamerTag, 2, false);
                        }
                    } else
                    {
                        API.SetMpGamerTagVisibility(gamerTag, 0, false);
                        API.SetMpGamerTagVisibility(gamerTag, 6, false);
                        API.SetMpGamerTagVisibility(gamerTag, 7, false);
                        API.SetMpGamerTagVisibility(gamerTag, 8, false);
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

        private static bool IsOtherPlayerWithinDistance(Player player)
        {
            return API.Vdist2(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, player.Character.Position.X, player.Character.Position.Y, player.Character.Position.Z) <= distance;
        }

        // Used to check if the player has a clear line of sight to another player
        private static bool IsOtherPlayerVisible(Player player)
        {
            bool isIntersecting = false;
            Vector3 endCoords = new Vector3(), surfaceNormal = new Vector3();
            int entity = 0;
            int raycast = API.StartShapeTestRay(Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, player.Character.Position.X, player.Character.Position.Y, player.Character.Position.Z, 1, 0, 0);
            API.GetShapeTestResult(raycast, ref isIntersecting, ref endCoords, ref surfaceNormal, ref entity);
            return !isIntersecting;
        }

        private static bool IsPlayerAimingAtOtherPlayer(Player player)
        {

            if (API.IsPlayerFreeAimingAtEntity(Game.Player.Handle, player.Character.Handle) || API.IsPlayerTargettingEntity(Game.Player.Handle, player.Character.Handle))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
