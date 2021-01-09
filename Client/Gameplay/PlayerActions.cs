using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Gameplay
{
    class PlayerActions : BaseScript
    {
        private bool isPointing = false;
        private async void StartPointing()
        {
            RequestAnimDict("anim@mp_point");
            while (!HasAnimDictLoaded("anim@mp_point"))
            {
                await Delay(0);
            }
            SetPedCurrentWeaponVisible(Game.Player.Character.Handle, false, true, true, true);
            SetPedConfigFlag(Game.Player.Character.Handle, 36, true);
            TaskMoveNetworkByName(Game.Player.Character.Handle, "task_mp_pointing", 0.5f, false, "anim@mp_point", 24);
            RemoveAnimDict("anim@mp_point");
        }

        private void StopPointing()
        {
            isPointing = false;
            RequestTaskMoveNetworkStateTransition(Game.Player.Character.Handle, "Stop");
            if (!IsPedInjured(Game.Player.Character.Handle))
            {
                ClearPedSecondaryTask(Game.Player.Character.Handle);
            }
            if (!IsPedInAnyVehicle(Game.Player.Character.Handle, true))
            {
                SetPedCurrentWeaponVisible(Game.Player.Character.Handle, true, true, true, true);
            }
            Game.Player.Character.SetConfigFlag(36, false);
            ClearPedSecondaryTask(Game.Player.Character.Handle);
        }

        private void MoveFinger()
        {
            float camPitch = GetGameplayCamRelativePitch();
            if (camPitch < -70f)
            {
                camPitch = -70f;
            }
            else if (camPitch > 42f)
            {
                camPitch = 42f;
            }
            camPitch = (camPitch + 70f) / 112f;

            float camHeading = GetGameplayCamRelativeHeading();
            float cosCamHeading = Cos(camHeading);
            float sinCamHeading = Sin(camHeading);
            if (camHeading < -180f)
            {
                camHeading = -180f;
            }
            else if (camHeading > 180f)
            {
                camHeading = 180f;
            }
            camHeading = (camHeading + 180f) / 360f;

            bool blocked = false;
            bool nn = false;
            int entity = 0;

            Vector3 coords = GetOffsetFromEntityInWorldCoords(Game.Player.Character.Handle, (cosCamHeading * -0.2f) - (sinCamHeading * (0.4f * camHeading + 0.3f)), (sinCamHeading * -0.2f) + (cosCamHeading * (0.4f * camHeading + 0.3f)), 0.6f);
            int raycast = Cast_3dRayPointToPoint(coords.X, coords.Y, coords.Z - 0.2f, coords.X, coords.Y, coords.Z + 0.2f, 0.4f, 95, Game.Player.Character.Handle, 7);
            GetRaycastResult(raycast, ref nn, ref coords, ref coords, ref entity);

            SetTaskMoveNetworkSignalFloat(Game.Player.Character.Handle, "Pitch", camPitch);
            SetTaskMoveNetworkSignalFloat(Game.Player.Character.Handle, "Heading", camHeading * -1.0f + 1.0f);
            SetTaskMoveNetworkSignalBool(Game.Player.Character.Handle, "isBlocked", blocked);
            SetTaskMoveNetworkSignalBool(Game.Player.Character.Handle, "isFirstPerson", N_0xee778f8c7e1142e2(N_0x19cafa3c87f7c2ff()) == 4);
        }

        private async void FingerPointing()
        {
            if (Game.IsControlJustPressed(0, Control.SpecialAbilitySecondary) && Game.Player.Character.IsOnFoot)
            {
                isPointing = !isPointing;

                if (isPointing)
                {
                    await Delay(200);
                    StartPointing();
                }
                else
                {
                    StopPointing();
                }
            }

            if (IsTaskMoveNetworkActive(Game.Player.Character.Handle) && !isPointing)
            {
                StopPointing();
            }
            else if (IsTaskMoveNetworkActive(Game.Player.Character.Handle))
            {
                if (!Game.Player.Character.IsOnFoot || Game.Player.Character.IsAiming || IsPedArmed(Game.Player.Character.Handle, 1) || IsPedArmed(Game.Player.Character.Handle, 2) || IsPedArmed(Game.Player.Character.Handle, 4))
                {
                    StopPointing();
                }
                else
                {
                    MoveFinger();
                }
            }
        }

        public void Update()
        {
            FingerPointing();
        }
    }
}
