using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Menus.InteractionMenu
{
    class Vehicles
    {
        private Menu menu;
        private Menu requestVehicleMenu;
        private readonly List<Menu> vehicleClassMenuList = new List<Menu>()
        {
            new Menu("Sports"),
            new Menu("Muscle"),
            new Menu("Super"),
            new Menu("Sports Classics"),
            new Menu("Sedans"),
            new Menu("SUVs"),
            new Menu("Compacts"),
            new Menu("Coupes"),
            new Menu("Motorcycles"),
            new Menu("Off-Road"),
            new Menu("Open Wheel"),
            new Menu("Cycles"),
            new Menu("Vans"),
            new Menu("Utility"),
            new Menu("Industrial"),
            new Menu("Service"),
            new Menu("Commercial"),
            new Menu("Emergency"),
            new Menu("Military")
        };
        private MenuItem vehicleItem;
        private List<MenuItem> vehicleItemList = new List<MenuItem>();

        private static Vehicle currentVehicle;

        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }
        public void CreateMenu()
        {
            menu = new Menu(Game.Player.Name, "Vehicles");

            CreateRequestVehicleMenu();

            CreateClassMenus("Compacts", Data.VehicleData.Compacts);
            CreateClassMenus("Muscle", Data.VehicleData.Muscle);
        }

        private void CreateRequestVehicleMenu()
        {
            requestVehicleMenu = new Menu(Game.Player.Name, "Request Vehicle");
            MenuController.AddSubmenu(menu, requestVehicleMenu);
            MenuItem requestVehicleButton = new MenuItem("Request Vehicle", "Fuck");
            menu.AddMenuItem(requestVehicleButton);
            MenuController.BindMenuItem(menu, requestVehicleMenu, requestVehicleButton);
        }

        //--------------------------------------------------------------------
        // CreateClassMenus - Creates a submenu for a given vehicle class
        //--------------------------------------------------------------------
        private void CreateClassMenus(string title, List<string> vehiclesList)
        {
            VehicleClassMenu classMenu = new VehicleClassMenu(title, vehiclesList);

            MenuController.AddSubmenu(requestVehicleMenu, classMenu.GetMenu());
            MenuItem classMenuButton = new MenuItem(title, "Fuck");
            requestVehicleMenu.AddMenuItem(classMenuButton);
            MenuController.BindMenuItem(requestVehicleMenu, classMenu.GetMenu(), classMenuButton);
        }

        public async static void SummonVehicle(string modelName)
        {
            if (currentVehicle != null)
            {
                await DeleteVehicle();
            }

            Model model = new Model(API.GetHashKey(modelName));

            Vector3 playerPosition = Game.PlayerPed.Position;
            Vector3 spawnLocation = new Vector3();
            float spawnHeading = 0f;
            int unusedVar = 0;
            API.GetNthClosestVehicleNodeWithHeading(playerPosition.X, playerPosition.Y, playerPosition.Z, 10, ref spawnLocation, ref spawnHeading, ref unusedVar, 9, 3.0f, 2.5f);
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

        private async static Task DeleteVehicle()
        {
            currentVehicle.AttachedBlip.Delete();
            API.NetworkFadeOutEntity(currentVehicle.Handle, true, false);
            await BaseScript.Delay(1000);
            currentVehicle.Delete();
        }

    }

    class VehicleClassMenu
    {
        private Menu menu;
        private MenuItem vehicleItem;
        private List<MenuItem> vehicleItemList = new List<MenuItem>();

        public VehicleClassMenu(string title, List<string> vehiclesList)
        {
            menu = new Menu(Game.Player.Name, title);

            foreach (string vehicleModel in vehiclesList)
            {
                vehicleItem = new MenuItem(API.GetLabelText(vehicleModel));
                if(vehicleItem.Text != "NULL")
                {
                    vehicleItem.ItemData = vehicleModel;
                    menu.AddMenuItem(vehicleItem);
                    vehicleItemList.Add(vehicleItem);
                }
            }

            menu.OnItemSelect += (sender, item, index) =>
            {
                Vehicles.SummonVehicle(vehicleItemList[index].ItemData);
            };
        }

        public Menu GetMenu()
        {
            return menu;
        }
    }
}
