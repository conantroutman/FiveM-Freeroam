using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;
using MenuAPI;
using Client.Menus.InteractionMenu;

namespace Client
{
    public class MainClient : BaseScript
    {
        private static InteractionMenu InteractionMenu { get; set; }
        public MainClient()
        {
            InteractionMenu = new InteractionMenu();
            InteractionMenu.CreateMenu();
            API.StatSetInt((uint)API.GetHashKey("MP0_STAMINA"), 100, true);
            API.RegisterCommand("passive", new Action(Gameplay.PassiveMode.Enable), false);
            Client.Players.Colors.Setup();
            WorldContent.WeaponPickups.CreatePickups();
            HUD.Blips.Create();
            HUD.GamerTags.Create();
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            //Player.Loop();
            HUD.Blips.Update();
            HUD.GamerTags.Update();
        }

    }
}
