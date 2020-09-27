using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace Client.Gameplay
{
    public class PersonalVehicleController : BaseScript
    {
        public Vehicle currentVehicle { get; private set; }
        private static bool isNotInVehicle = true;

        public PersonalVehicleController()
        {
            EventHandlers["enteredPersonalVehicle"] += new Action(() => { Debug.WriteLine("Entered vehicle"); });
            EventHandlers["exitedPersonalVehicle"] += new Action(() => { Debug.WriteLine("Exited vehicle"); });
        }

        public Vehicle GetCurrentVehicle()
        {
            return currentVehicle;
        }

        public async void SummonVehicle(string modelName)
        {
            if (currentVehicle != null)
            {
                await DeleteVehicle();
            }

            Model model = new Model(API.GetHashKey(modelName));

            int spawnDistance = 10;
            Vector3 playerPosition = Game.PlayerPed.Position;
            Vector3 playerForwardVector = Game.PlayerPed.ForwardVector;
            playerPosition = playerPosition - (playerForwardVector * spawnDistance);

            Vector3 spawnLocation = new Vector3();
            float spawnHeading = 0f;
            int unusedVar = 0;
            int nth = 1;
            API.GetNthClosestVehicleNodeWithHeading(playerPosition.X, playerPosition.Y, playerPosition.Z, nth, ref spawnLocation, ref spawnHeading, ref unusedVar, 9, 3.0f, 2.5f);
            API.GetRoadSidePointWithHeading(spawnLocation.X, spawnLocation.Y, spawnLocation.Z, spawnHeading, ref spawnLocation);

            currentVehicle = await World.CreateVehicle(model, spawnLocation, spawnHeading);
            API.NetworkFadeInEntity(currentVehicle.Handle, true);
            currentVehicle.NeedsToBeHotwired = false;

            currentVehicle.AttachBlip().Sprite = BlipSprite.PersonalVehicleCar;
            currentVehicle.AttachedBlip.Name = "Personal Vehicle";
            currentVehicle.AttachedBlip.IsFlashing = true;
            await BaseScript.Delay(5000);
            currentVehicle.AttachedBlip.IsFlashing = false;
        }

        private async Task DeleteVehicle()
        {
            if (currentVehicle.AttachedBlip != null) currentVehicle.AttachedBlip.Delete();
            API.NetworkFadeOutEntity(currentVehicle.Handle, true, false);
            await BaseScript.Delay(1000);
            currentVehicle.Delete();
        }

        public async void OnTick()
        {
            // The following if statement handles hiding and unhiding of the vehicle blip as the player enters and exits the vehicle.

            // Enter vehicle
            if (Game.Player.Character.IsInVehicle() && Game.Player.Character.CurrentVehicle == currentVehicle)
            {
                if (isNotInVehicle)
                {
                    isNotInVehicle = !isNotInVehicle;
                    currentVehicle.AttachedBlip.Delete();
                    TriggerEvent("enteredPersonalVehicle");
                }
            }
            // Exit vehicle
            else if (!Game.Player.Character.IsInVehicle() && Game.Player.LastVehicle == currentVehicle)
            {
                if (!isNotInVehicle)
                {
                    isNotInVehicle = !isNotInVehicle;
                    currentVehicle.AttachBlip().Sprite = BlipSprite.PersonalVehicleCar;
                    currentVehicle.AttachedBlip.Name = "Personal Vehicle";
                    TriggerEvent("exitedPersonalVehicle");
                }
            }
        }

        [EventHandler("onResourceStop")]
        private void OnResourceStop(string name)
        {
            if (name == API.GetCurrentResourceName())
            {
                // Do stuff when the resource stops
            }
        }
    }
}
