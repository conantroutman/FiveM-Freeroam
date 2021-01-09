using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;
using Client.Menus.InteractionMenu;
using Client.UIComponents;
using Client.HUD;
using Client.Gameplay;
using Client.Managers;
using Client.Activities;
using System;

namespace Client
{
    public class MainClient : BaseScript
    {
        private HUDManager HUDManager = new HUDManager();
        private SpawnManager SpawnManager = new SpawnManager();
        private TimeTrials TimeTrials = new TimeTrials();
        private BaseJumping baseJumping = new BaseJumping();
        private PlayerActions playerActions = new PlayerActions();
        public static PersonalVehicleController PersonalVehicleController = new PersonalVehicleController();

        
        
        
        public MainClient()
        {
            API.SetRadarBigmapEnabled(false, false);
            API.NetworkSetFriendlyFireOption(true);
            API.SetCanAttackFriendly(Game.Player.Character.Handle, true, true);
            Tick += OnTick;

            EventHandlers["playerEnteredVehicle"] += new Action<int>(OnPlayerEnteredVehicle);
        }

        private void OnPlayerEnteredVehicle(int serverId)
        {
            if (Game.Player.Handle == Players[serverId].Handle)
            {
                if (Game.Player.Character.CurrentVehicle.ClassType == VehicleClass.Helicopters || Game.Player.Character.CurrentVehicle.ClassType == VehicleClass.Planes)
                {
                    API.GiveWeaponToPed(Game.Player.Character.Handle, (uint)API.GetHashKey("gadget_parachute"), 1, false, false);
                }
            }
        }

        private async Task OnTick()
        {
            HUD.GamerTags.Update();
            TimeTrials.Update();
            baseJumping.Update();
            PersonalVehicleController.OnTick();
            HUDManager.Update();
            WorldContent.WeaponPickups.Update();
            playerActions.Update();
        }
    }
}
