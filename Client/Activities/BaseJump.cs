using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Drawing;
using System.Threading.Tasks;

namespace Client.Activities
{
    class BaseJump : BaseScript
    {
        private Vector3 markerLocation, teleportLocation;
        private int blipSprite = 94, blip;
        private float heading;
        private bool isReadyToJump = false;
        public BaseJump(Vector3 markerLocation, Vector3 teleportLocation, float heading)
        {
            this.markerLocation = markerLocation - new Vector3(0f, 0f, 1f);
            this.teleportLocation = teleportLocation;
            this.heading = heading;
            API.GetGroundZFor_3dCoord(teleportLocation.X, teleportLocation.Y, teleportLocation.Z, ref this.teleportLocation.Z, false);

            this.blip = API.AddBlipForCoord(this.markerLocation.X, this.markerLocation.Y, this.markerLocation.Z);
            API.SetBlipSprite(this.blip, this.blipSprite);
            API.SetBlipAsShortRange(this.blip, true);
        }

        private void GiveParachute()
        {
            API.GiveWeaponToPed(Game.Player.Character.Handle, (uint)API.GetHashKey("gadget_parachute"), 1, false, false);
            API.SetPedComponentVariation(Game.Player.Character.Handle, 5, 1, 1, 0);
        }

        private async Task Teleport()
        {
            Screen.Fading.FadeOut(1000);
            API.NetworkFadeOutEntity(Game.Player.Character.Handle, true, false);
            await BaseScript.Delay(2000);
            Game.Player.Character.Position = teleportLocation;
            Game.Player.Character.Heading = heading;
            API.NetworkFadeInEntity(Game.Player.Character.Handle, false);
            Screen.Fading.FadeIn(1000);
            isReadyToJump = true;
            TriggerEvent("basejumping:readyToJump");
        }

        private bool IsPlayerWithinRange()
        {
            /*if (API.Vdist2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, markerLocation.X, markerLocation.Y, markerLocation.Z) < 1000f)
            {
                return true;
            } else
            {
                return false;
            }*/
            return true;
        }

        public async Task DrawMarker()
        {
            API.Wait(0);

            if (IsPlayerWithinRange())
            {
                World.DrawMarker(MarkerType.VerticalCylinder, markerLocation, new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), Color.FromArgb(150, 132, 102, 226));
                //World.DrawMarker(MarkerType.ThickChevronUp, startPosition + new Vector3(0f, 0f, 3f), finishPosition, new Vector3(89.999f, 90f, 0f), new Vector3(1f, 1f, 1f), markerColorOuter);
                //API.DrawLightWithRangeAndShadow(startPosition.X, startPosition.Y, startPosition.Z + 1.7f, 36, 120, 255, 10f, 5f, 64f);

                if (API.Vdist2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, markerLocation.X, markerLocation.Y, markerLocation.Z+1f) < 1f)
                {
                    Game.DisableControlThisFrame(1, Control.VehicleHorn);

                    // Can't start time trial on foot
                    if (Game.Player.Character.IsInVehicle())
                    {
                        //Screen.DisplayHelpTextThisFrame();
                    }
                    else
                    {
                        Screen.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to jump from this building.");

                        if (Game.IsControlJustPressed(0, Control.Context))
                        {
                            await Teleport();
                            GiveParachute();
                        }
                    }
                }
            }
        }
    }
}
