using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Client.HUD
{
    class WastedScreen
    {
        private string SCREEN_EFFECT = "DeathFailOut";
        private string SOUND_BED = "Bed";
        private string SOUND_TEXT = "TextHit";
        private string SOUNDSET = "WastedSounds";
        private string SHAKE_EFFECT = "DEATH_FAIL_IN_EFFECT_SHAKE";

        private bool lockSound = false;


        private string message;

        private Ped playerPed;

        private Scaleform wastedScaleform;

        public WastedScreen()
        {
            playerPed = Game.PlayerPed;
            API.StopScreenEffect(SCREEN_EFFECT);
            API.StopGameplayCamShaking(true);
        }

        private async Task ShowWastedScreen()
        {
            API.StartScreenEffect(SCREEN_EFFECT, 0, false);
            if (!lockSound)
            {
                Audio.PlaySoundFrontend(SOUND_BED, SOUNDSET);
                lockSound = true;
            }

            API.ShakeGameplayCam(SHAKE_EFFECT, 1.0f);

            wastedScaleform = new Scaleform("MP_BIG_MESSAGE_FREEMODE");
            while (!wastedScaleform.IsLoaded) { await BaseScript.Delay(0); }

            if (message != null) { wastedScaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", "Wasted", message); }
            else { wastedScaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", "Wasted"); }

            await BaseScript.Delay(500);

            Audio.PlaySoundFrontend(SOUND_TEXT, SOUNDSET);
            if (API.IsEntityDead(playerPed.Handle))
            {
                API.DrawScaleformMovieFullscreen(wastedScaleform.Handle, 255, 255, 255, 255, 0);
                await BaseScript.Delay(0);
            }

            API.StopScreenEffect(SCREEN_EFFECT);
            lockSound = false;
        }

        public async void Loop()
        {
            if (API.IsEntityDead(playerPed.Handle))
            {
                await ShowWastedScreen();
            }
        }
    }
}
