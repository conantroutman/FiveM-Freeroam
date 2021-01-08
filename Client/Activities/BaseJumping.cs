using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;

namespace Client.Activities
{
    class BaseJumping : BaseScript
    {
        private bool isBaseJumping = false;
        private bool isReadyToJump = false;
        private bool hasOpenedParachute = false;
        private bool hasLanded = false;
        private long timerOpenParachute = 0;
        private long timerLand = 0;
        private int distance = 0;
        private int glideTime = 0;

        private static List<BaseJump> BaseJumpList = new List<BaseJump>()
        {
            new BaseJump(new Vector3(-736.9f, -2276.04f, 13.44f), new Vector3(-713.25f, -2266.48f, 88.74f), 0f),
            new BaseJump(new Vector3(-862.42f, -2141.44f, 8.93f), new Vector3(-878.19f, -2179.76f, 96.26f), 0f),
            new BaseJump(new Vector3(-231.42f, -852.1f, 30.68f), new Vector3(-231.42f, -852.1f, 30.68f), 0f),
            new BaseJump(new Vector3(-213.23f, -727.99f, 33.54f), new Vector3(-217.98f, -746.32f, 221.5f), 109.5f),
            new BaseJump(new Vector3(-116.79f, -604.72f, 36.28f), new Vector3(-139.53f, -586.77f, 211.78f), 0f),
            new BaseJump(new Vector3(6.8f, -933.47f, 29.91f), new Vector3(9.78f, -926.57f, 123.08f), 0f),
            new BaseJump(new Vector3(5.1f, -706.91f, 45.97f), new Vector3(1.97f, -687.23f, 250.41f), 0f),
            new BaseJump(new Vector3(-66.71f, -801.64f, 44.23f), new Vector3(-70.08f, -805.14f, 322.33f), 342.5f),
            new BaseJump(new Vector3(104.61f, -744.53f, 45.75f), new Vector3(104.61f, -744.53f, 45.75f), 0f),
            new BaseJump(new Vector3(112.59f, -627.69f, 44.23f), new Vector3(103.88f, -639.74f, 259.75f), 69.09f),
            new BaseJump(new Vector3(-925.12f, -458.07f, 37.38f), new Vector3(-912.31f, -450.47f, 168.12f), 116.47f), // Weazel News
            new BaseJump(new Vector3(-828.07f, -698.49f, 28.06f), new Vector3(-925.12f, -458.07f, 37.38f), 0f),
            new BaseJump(new Vector3(121.68f, -878.38f, 31.12f), new Vector3(123.36f, -880.61f, 134.77f), 248.81f),
            new BaseJump(new Vector3(-773.79f, 311.41f, 85.7f), new Vector3(-805.14f, 330.12f, 233.67f), 86.96f),
        };
        public BaseJumping()
        {
            EventHandlers["basejumping:onLanding"] += new Action<int, int, int>(OnFinishedBaseJumping);
            EventHandlers["playerDied"] += new Action(CancelBaseJump);
            EventHandlers["playerKilled"] += new Action(CancelBaseJump);
            //EventHandlers["goFuckYourself"] += new Action(() => { isReadyToJump = true; });
        }

        public void Update()
        {
            BaseJumpList.ForEach(baseJump => {
                baseJump.DrawMarker();
            });

            if (Game.Player.Character.ParachuteState == ParachuteState.None)
            {
                isBaseJumping = false;
            } else if (Game.Player.Character.ParachuteState == ParachuteState.FreeFalling)
            {
                isBaseJumping = true;
            }

            if (isBaseJumping)
            {
                if (!hasOpenedParachute && Game.Player.Character.ParachuteState == ParachuteState.Gliding)
                {
                    hasOpenedParachute = true;
                    timerOpenParachute = API.GetGameTimer();
                    distance = GetDistanceToGround();
                    Debug.WriteLine($"Opened parachute {distance} above ground");
                } else if (hasOpenedParachute && Game.Player.Character.ParachuteState == ParachuteState.LandingOrFallingToDoom) {
                    hasLanded = true;
                    isBaseJumping = false;
                    hasOpenedParachute = false;
                    timerLand = API.GetGameTimer();
                    // Time is measured in milliseconds, so divide by 1000 to get seconds
                    glideTime = (int)(timerLand - timerOpenParachute) / 1000;
                    TriggerServerEvent("basejumping:playerLanded", distance, glideTime);
                }
            }
        }

        private int GetDistanceToGround()
        {
            Vector3 playerPosition = Game.Player.Character.Position;
            float groundZ = 0f;
            API.GetGroundZFor_3dCoord(playerPosition.X, playerPosition.Y, playerPosition.Z, ref groundZ, false);
            int distanceToGround = (int)(playerPosition.Z - groundZ);

            return distanceToGround;
        }

        // Display notifications when a player has completed a base jump.
        private void OnFinishedBaseJumping(int serverId, int _distance, int _glideTime)
        {
            Player player = Players[serverId];
            string nameString = player.Handle == Game.Player.Handle ? "You" : $"~HUD_COLOUR_NET_PLAYER{serverId}~<C>{player.Name}</C>~w~";
            string distanceString = API.ShouldUseMetricMeasurements() ? $"{_distance}m" : $"{(int)(_distance * 3.2808f)}ft";
            string secondString = _glideTime > 1 ? "seconds" : "second";
            Screen.ShowNotification($"{nameString} activated parachute {distanceString} before landing.");
            if (_glideTime > 0) { Screen.ShowNotification($"{nameString} glided for {_glideTime} {secondString}."); }
        }

        private void CancelBaseJump()
        {
            hasLanded = true;
            isBaseJumping = false;
            hasOpenedParachute = false;
        }

        [EventHandler("basejumping:readyToJump")]
        private void ReadyToJump()
        {
            isReadyToJump = true;
        }
    }
}
