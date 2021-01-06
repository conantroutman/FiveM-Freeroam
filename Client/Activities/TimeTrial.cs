using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Drawing;

namespace Client.Activities
{
    class TimeTrial
    {
        private float parTime;
        private Vector3 startPosition;
        private float startHeading;
        private Vector3 finishPosition;
        private float finishHeading;
        private string timeTrialName;

        private readonly string timeTrialText = API.GetLabelText("AMTT_TRIAL");
        private readonly string parTimeText = API.GetLabelText("AMTT_STARTPAR");
        private readonly Color markerColorInner = Color.FromArgb(64, 132, 102, 226);
        private readonly Color markerColorOuter = Color.FromArgb(150, 132, 102, 226);
        private Scaleform scaleform = new Scaleform("MP_MISSION_NAME_FREEMODE");
        private bool isRunning = false;
        private Blip finishBlip;
        public TimeTrial(float par, Vector3 start, float startHeading, Vector3 finish, float finishHeading, string label)
        {
            this.parTime = par;
            this.startPosition = start;
            this.startHeading = startHeading;
            this.finishPosition = finish;
            this.finishHeading = finishHeading;
            this.timeTrialName = API.GetLabelText(label);

            // Make sure the markers are placed on the ground
            API.GetGroundZFor_3dCoord(startPosition.X, startPosition.Y, startPosition.Z, ref startPosition.Z, false);
            API.GetGroundZFor_3dCoord(finishPosition.X, finishPosition.Y, finishPosition.Z, ref finishPosition.Z, false);
            DrawIcon();
            CreateStartBlip();
        }

        public void Start()
        {
            isRunning = true;
            CreateFinishBlip();
            Screen.ShowSubtitle(API.GetLabelText("AMTT_GOTO"), 2500);
        }

        public void Finish()
        {
            Screen.ShowNotification(Game.Player.Name + API.GetLabelText("AMTT_BEATNO"));
        }

        public void Cancel()
        {
            isRunning = false;
            DeleteFinishBlip();
        }

        public void Reset()
        {

        }

        private void DeleteFinishBlip()
        {
            finishBlip.Delete();
        }

        private void CreateFinishBlip()
        {
            finishBlip = World.CreateBlip(finishPosition);
            finishBlip.Color = BlipColor.Yellow;
            finishBlip.Priority = 12;
            finishBlip.Name = API.GetLabelText("AMTT_DESTIN");
        }

        private void CreateStartBlip()
        {
            Blip blip = World.CreateBlip(startPosition);
            blip.Priority = 5;
            blip.Sprite = BlipSprite.Stopwatch;
            blip.Name = API.GetLabelText("AMTT_BLIP");
            API.SetBlipAsMissionCreatorBlip(blip.Handle, true);
            blip.IsShortRange = true;
            blip.Color = (BlipColor)7;
            API.SetBlipHighDetail(blip.Handle, true);
        }

        private bool IsVehicleBlacklisted()
        {
            Vehicle veh = Game.Player.Character.CurrentVehicle;
            switch (veh.ClassType)
            {
                case VehicleClass.Boats:
                    return true;
                case VehicleClass.Helicopters:
                    return true;
                case VehicleClass.Planes:
                    return true;
                default:
                    return false;
            }
        }

        private void DrawIcon()
        {
            float radius = ((2.6f * 2f) * 1.04f);
            int checkpoint = API.CreateCheckpoint(51, startPosition.X, startPosition.Y, startPosition.Z, finishPosition.X, finishPosition.Y, finishPosition.Z, radius, 64, 132, 102, 226, 0);
            API.SetCheckpointCylinderHeight(checkpoint, 7.5f, 7.5f, radius);
            Debug.WriteLine(checkpoint.ToString());
        }

        public async void DrawMarker()
        {
            API.Wait(0);
            World.DrawMarker(MarkerType.VerticalCylinder, startPosition, new Vector3(), new Vector3(), new Vector3(2.6f * 2f, 2.6f * 2f, 1f), markerColorOuter);
            World.DrawMarker(MarkerType.ThickChevronUp, startPosition + new Vector3(0f, 0f, 3f), finishPosition, new Vector3(89.999f, 90f, 0f), new Vector3(1f, 1f, 1f), markerColorOuter);
            scaleform.CallFunction("SET_MISSION_INFO", timeTrialName, $"~p~" + timeTrialText, "", "", "", false, "", "", "", parTimeText + FormatParTime());
            scaleform.Render3D(startPosition + new Vector3(0f, 0f, 0.75f), GameplayCamera.Rotation, new Vector3(3f, 3f, 3f));
            API.DrawLightWithRangeAndShadow(startPosition.X, startPosition.Y, startPosition.Z + 1.7f, 36, 120, 255, 10f, 5f, 64f);

            if (API.Vdist2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, startPosition.X, startPosition.Y, startPosition.Z) < 10f)
            {
                Game.DisableControlThisFrame(1, Control.VehicleHorn);

                if (!isRunning)
                {
                    // Can't start time trial on foot
                    if (!Game.Player.Character.IsInVehicle())
                    {
                        Screen.DisplayHelpTextThisFrame(API.GetLabelText("AMTT_CORONA"));
                    }
                    // Can't start time trial in non-land vehicle
                    else if (IsVehicleBlacklisted())
                    {
                        Screen.DisplayHelpTextThisFrame(API.GetLabelText("AMTT_LAND"));
                    }
                    // Can't start time trial with passengers
                    else if (Game.Player.Character.CurrentVehicle.PassengerCount > 0)
                    {
                        Screen.DisplayHelpTextThisFrame(API.GetLabelText("AMTT_NOPASSN"));
                    }
                    else
                    {
                        Screen.DisplayHelpTextThisFrame(API.GetLabelText("AMTT_PRESS"));

                        if (Game.IsControlJustPressed(0, Control.Context))
                        {
                            Start();
                        }
                    }
                }
            }


            // Surrounding circle
            

            if (isRunning)
            {
                // Draw finish marker
            }
        }

        private string FormatParTime()
        {
            int minutes = (int)Math.Floor(parTime) / 60;
            int seconds = (int)Math.Floor(parTime) % 60;
            int milliseconds = (int)((parTime % 1) * 10);
            return $"{minutes.ToString().PadLeft(2, '0')}:{seconds.ToString().PadLeft(2, '0')}.{milliseconds.ToString().PadRight(3, '0')}";
        }
    }
}
