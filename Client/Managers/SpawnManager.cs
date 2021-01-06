using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Managers
{
    class SpawnManager : BaseScript
    {
        private bool isFirstSpawn = true;
        private Vector3 lastDeathLocation = new Vector3();
        private bool isRespawning = true;
        public SpawnManager()
        {
            EventHandlers["playerSpawned"] += new Action(PlayerSetup);

            Exports["spawnmanager"].setAutoSpawnCallback(new Action(Respawn));
        }
        private static void PlayerSetup()
        {
            // Infinite sprint, like in GTAO
            API.StatSetInt((uint)API.GetHashKey("MP0_STAMINA"), 100, true);
            API.StatSetInt((uint)API.GetHashKey("MP0_SHOOTING_ABILITY"), 100, true);
            API.StatSetInt((uint)API.GetHashKey("MP0_STRENGTH"), 100, true);
            API.StatSetInt((uint)API.GetHashKey("MP0_STEALTH_ABILITY"), 100, true);
            API.StatSetInt((uint)API.GetHashKey("MP0_FLYING_ABILITY"), 100, true);
            API.StatSetInt((uint)API.GetHashKey("MP0_WHEELIE_ABILITY"), 100, true);
            API.StatSetInt((uint)API.GetHashKey("MP0_LUNG_CAPACITY"), 100, true);

            // Give the player a pistol
            API.GiveWeaponToPed(Game.Player.Character.Handle, (uint)WeaponHash.Pistol, 12 * 7, false, false);

            API.ReplaceHudColour(116, 27 + Game.Player.ServerId);
        }

        private static void SetPlayerSkin()
        {
            // Use player-based random seed for Rust-style player character generation
            Random rand = new Random(Game.Player.Name.GetHashCode());
            int player = Game.PlayerPed.Handle;

            // Set default ped variation to not make player invisible
            API.SetPedDefaultComponentVariation(player);

            // Generate a random face + skin tone
            int father = rand.Next(0, 20);
            int mother = rand.Next(21, 45);
            API.SetPedHeadBlendData(player, father, mother, 0, father, mother, 0, (float)rand.NextDouble(), (float)rand.NextDouble(), 0, false);

            // Generate a random hair style + color
            int hairStyle = rand.Next(0, 74);
            int hairColor = rand.Next(0, 63);
            int beardStyle = rand.Next(0, 28);
            int beardOpacity = rand.Next(0, 255);
            int eyebrowStyle = rand.Next(0, 33);
            int eyebrowOpacity = rand.Next(0, 255);
            int chesthairStyle = rand.Next(0, 16);
            int chesthairOpacity = rand.Next(0, 255);
            if (hairStyle == 23) { hairStyle++; };
            API.SetPedComponentVariation(player, 2, hairStyle, 0, 0);
            API.SetPedHairColor(player, hairColor, hairColor);
            API.SetPedHeadOverlay(player, 1, beardStyle, beardOpacity);
            API.SetPedHeadOverlayColor(player, 1, 1, hairColor, hairColor);
            API.SetPedHeadOverlay(player, 2, eyebrowStyle, eyebrowOpacity);
            API.SetPedHeadOverlayColor(player, 2, 1, hairColor, hairColor);
            API.SetPedHeadOverlay(player, 2, chesthairStyle, chesthairOpacity);
            API.SetPedHeadOverlayColor(player, 2, 1, hairColor, hairColor);

            // Ageing
            int ageingStyle = rand.Next(0, 14);
            int ageingOpacity = rand.Next(0, 255);
            API.SetPedHeadOverlay(player, 2, ageingStyle, ageingOpacity);

        }
        private void Respawn()
        {
            if (isFirstSpawn)
            {
                Exports["spawnmanager"].spawnPlayer(new
                {
                    x = -1135.707275,
                    y = -1987.154175,
                    z = 12.976217,
                    heading = 260.0,
                    model = "mp_m_freemode_01",
                    skipFade = false
                });
                isFirstSpawn = !isFirstSpawn;
            }
            // Respawn near death location
            else if (!isFirstSpawn && Game.Player.IsDead)
            {
                Vector3 deathLocation = Game.PlayerPed.Position;
                Vector3 randomCoord = new Vector3();
                Vector3 respawnLocation = new Vector3();
                API.GetNthClosestVehicleNode(deathLocation.X, deathLocation.Y, deathLocation.Z, 20, ref randomCoord, 0, 0, 0);
                API.GetPointOnRoadSide(randomCoord.X, randomCoord.Y, randomCoord.Z, 0, ref respawnLocation);
                //API.GetSafeCoordForPed(respawnLocation.X, respawnLocation.Y, respawnLocation.Z, true, ref respawnLocation, 16);

                Exports["spawnmanager"].spawnPlayer(new
                {
                    x = respawnLocation.X,
                    y = respawnLocation.Y,
                    z = respawnLocation.Z,
                    heading = 260.0,
                    skipFade = false
                });
            } else if (!Game.Player.IsDead)
            {
                Game.PlayerPed.ClearBloodDamage();
                Game.PlayerPed.ResetVisibleDamage();
            }
            SetPlayerSkin();
        }
    }
}