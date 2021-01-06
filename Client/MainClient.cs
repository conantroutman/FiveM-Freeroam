using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;
using Client.Menus.InteractionMenu;
using Client.UIComponents;
using Client.HUD;
using Client.Gameplay;
using Client.Managers;
using Client.Activities;

namespace Client
{
    public class MainClient : BaseScript
    {
        private HUDManager HUDManager = new HUDManager();
        private SpawnManager SpawnManager = new SpawnManager();
        private TimeTrials TimeTrials = new TimeTrials();
        public static PersonalVehicleController PersonalVehicleController = new PersonalVehicleController();
        
        
        public MainClient()
        {
            API.SetRadarBigmapEnabled(false, false);
            API.NetworkSetFriendlyFireOption(true);
            API.SetCanAttackFriendly(Game.Player.Character.Handle, true, true);
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            //BlipController.Update();
            HUD.GamerTags.Update();
            TimeTrials.Update();
            PersonalVehicleController.OnTick();
            HUDManager.Update();
            WorldContent.WeaponPickups.Update();
        }

        
    }
}
