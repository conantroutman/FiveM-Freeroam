/*using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;

namespace Client.Managers
{
    class SpawnManager : BaseScript
    {
        private static bool autoSpawnEnabled = false;

        [EventHandler("getMapDirectives")]
        private void getMapDirectives(string name)
        {

        }

        private void LoadSpawns(string spawnString)
        {

        }

        private void AddSpawnPoint()
        {

        }

        private void FreezePlayer(Player player, bool freeze)
        {
            Ped playerPed = player.Character;
            API.SetPlayerControl(player.Handle, !freeze, 0);

            if (!freeze)
            {
                if (!playerPed.IsVisible) { playerPed.IsVisible = true; }
                if (!playerPed.IsInVehicle()) { playerPed.IsCollisionEnabled = true; }

                API.FreezeEntityPosition(playerPed.Handle, false);
                playerPed.IsInvincible = false;
            }
            else
            {
                if (playerPed.IsVisible) { playerPed.IsVisible = false; }

                playerPed.IsCollisionEnabled = false;
                API.FreezeEntityPosition(playerPed.Handle, true);
                playerPed.IsInvincible = true;

                if (!API.IsPedFatallyInjured(playerPed.Handle)) { playerPed.Task.ClearAllImmediately(); }
            }
        }

        private void LoadScene(Vector3 coordinates)
        {
            if (API.NewLoadSceneStart())
            {
                return;
            }

            API.NewLoadSceneStart(coordinates.X, coordinates.Y, coordinates.Z, 0f, 0f, 0f, 20f, 0);

            while (API.IsNewLoadSceneActive())
            {
                
            }
        }
    }
}
*/