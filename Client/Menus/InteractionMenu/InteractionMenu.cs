using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using System.Media;

namespace Client.Menus.InteractionMenu
{
    class InteractionMenu : BaseScript
    {
        private static Menu Menu { get; set; }
        private static QuickGPS QuickGPSItem;
        private static Inventory Inventory;
        private static Style Style;
        private static Vehicles Vehicles;
        private static MenuItem killYourselfItem;

        public InteractionMenu()
        {
            CreateMenu();
        }

        private async void CreateMenu()
        {
            while (!API.DoesEntityExist(Game.Player.Character.Handle))
            {
                await Delay(0);
            }


            Menu = new Menu(Game.Player.Name, "Interaction Menu");
            MenuController.AddMenu(Menu);
            MenuController.MainMenu = Menu;

            CreateMenuItems();
        }

        private void CreateMenuItems()
        {
            // Quick GPS
            QuickGPSItem = new QuickGPS();
            Menu.AddMenuItem(QuickGPSItem.GetItem());

            

            // Inventory
            Inventory = new Inventory();
            MenuController.AddSubmenu(Menu, Inventory.GetMenu());
            MenuItem InventoryButton = new MenuItem("Inventory", "Your Inventory contains carried items such as weapon ammo and snacks.");
            Menu.AddMenuItem(InventoryButton);
            MenuController.BindMenuItem(Menu, Inventory.GetMenu(), InventoryButton);

            // Style
            Style = new Style();
            MenuController.AddSubmenu(Menu, Style.GetMenu());
            MenuItem StyleButton = new MenuItem("Style", "View and change player options.");
            Menu.AddMenuItem(StyleButton);
            MenuController.BindMenuItem(Menu, Style.GetMenu(), StyleButton);

            // Style
            Vehicles = new Vehicles();
            MenuController.AddSubmenu(Menu, Vehicles.GetMenu());
            MenuItem VehiclesButton = new MenuItem("Vehicles", "View and change vehicle options.");
            Menu.AddMenuItem(VehiclesButton);
            MenuController.BindMenuItem(Menu, Vehicles.GetMenu(), VehiclesButton);

            // Kill Yourself
            killYourselfItem = new MenuItem("Kill Yourself", "Are you sure you want to do this?");
            Menu.AddMenuItem(killYourselfItem);

            // Event handlers
            Menu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
            {
                if (listItem == QuickGPSItem.GetItem())
                {
                    QuickGPSItem.SetQuickGPS(listIndex);
                }
            };

            Menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == killYourselfItem)
                {
                    CommitSuicide();
                }
            };
        }

        private void CommitSuicide()
        {
            Ped playerPed = Game.PlayerPed;
            playerPed.Kill();
        }
    }
}
