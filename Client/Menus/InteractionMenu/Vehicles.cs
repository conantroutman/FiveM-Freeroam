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

        private List<string> doorsList = new List<string>()
        {
            "None",
            "All",
            "Left Door",
            "Right Door",
            "Hood",
            "Trunk"
        };
        private MenuListItem doorsItem;

        private Menu remoteControlsMenu;

        private static Vehicle currentVehicle;

        private static bool toggleBlip = true;

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

            // Request Vehicle
            CreateRequestVehicleMenu();

            // Vehicle Doors
            doorsItem = new MenuListItem("Doors", doorsList, 0);
            menu.AddMenuItem(doorsItem);

            menu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
            {
                if (listItem == doorsItem)
                {
                    DoorControls(listIndex);
                }
            };

            // Remote Functions
            remoteControlsMenu = new Menu(Game.Player.Name, "Vehicle Remote Functions");
            MenuController.AddSubmenu(menu, remoteControlsMenu);
            MenuItem remoteControlsMenuButton = new MenuItem("Vehicle Remote Functions", "Select to adjust vehicle engine, lights and radio options" );
            menu.AddMenuItem(remoteControlsMenuButton);
            MenuController.BindMenuItem(menu, remoteControlsMenu, remoteControlsMenuButton);

            MenuListItem toggleEngineItem = new MenuListItem("Engine", new List<string>() { "Off", "On" }, 0, "Sets your vehicle's engine on or off.");
            MenuListItem toggleHeadlightsItem = new MenuListItem("Headlights", new List<string>() { "Off", "On" }, 0, "Sets your vehicle's headlights on or off.");
            MenuListItem toggleNeonItem = new MenuListItem("Neon", new List<string>() { "Off", "On" }, 0, "Sets your vehicle's neon lights on or off.");
            MenuListItem toggleRadioItem = new MenuListItem("Radio", new List<string>() { "Off", "On" }, 0, "Sets your vehicle's radio on or off.");

            remoteControlsMenu.AddMenuItem(toggleEngineItem);
            remoteControlsMenu.AddMenuItem(toggleHeadlightsItem);
            remoteControlsMenu.AddMenuItem(toggleNeonItem);
            remoteControlsMenu.AddMenuItem(toggleRadioItem);

            remoteControlsMenu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
            {
                if (listItem == toggleEngineItem)
                {
                    API.SetVehicleEngineOn(currentVehicle.Handle, listIndex == 1, true, false);
                }
                else if (listItem == toggleHeadlightsItem)
                {
                    currentVehicle.AreLightsOn = listIndex == 1;
                }
                else if (listItem == toggleNeonItem)
                {
                    API.SetVehicleNeonLightEnabled(currentVehicle.Handle, 0, listIndex == 1);
                    API.SetVehicleNeonLightEnabled(currentVehicle.Handle, 1, listIndex == 1);
                    API.SetVehicleNeonLightEnabled(currentVehicle.Handle, 2, listIndex == 1);
                    API.SetVehicleNeonLightEnabled(currentVehicle.Handle, 3, listIndex == 1);
                }
                else if (listItem == toggleRadioItem)
                {
                    API.SetVehicleRadioEnabled(currentVehicle.Handle, listIndex == 1);
                    API.SetVehRadioStation(currentVehicle.Handle, "RADIO_04_PUNK");
                    API.SetVehicleRadioLoud(currentVehicle.Handle, listIndex == 1);
                }
            };
        }

        private void CreateRequestVehicleMenu()
        {
            requestVehicleMenu = new Menu(Game.Player.Name, "Request Vehicle");
            MenuController.AddSubmenu(menu, requestVehicleMenu);
            MenuItem requestVehicleButton = new MenuItem("Request Vehicle", "Get the mechanic to deliver a vehicle of your choice.");
            menu.AddMenuItem(requestVehicleButton);
            MenuController.BindMenuItem(menu, requestVehicleMenu, requestVehicleButton);

            CreateClassMenus("Open Wheel", Data.VehicleData.OpenWheel);
            CreateClassMenus("Super", Data.VehicleData.Super);
            CreateClassMenus("Sports", Data.VehicleData.Sports);
            CreateClassMenus("Sports Classics", Data.VehicleData.SportsClassics);
            CreateClassMenus("Muscle", Data.VehicleData.Muscle);
            CreateClassMenus("Compacts", Data.VehicleData.Compacts);
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

        private void DoorControls(int index)
        {
            switch (index)
            {
                case 0:
                    foreach (VehicleDoor door in currentVehicle.Doors.GetAll())
                    {
                        door.Close();
                    }
                    break;
                case 1:
                    foreach (VehicleDoor door in currentVehicle.Doors.GetAll())
                    {
                        door.Open();
                    }
                    break;
                case 2:
                    currentVehicle.Doors.GetAll()[0].Open();
                    break;
                case 3:
                    currentVehicle.Doors.GetAll()[1].Open();
                    break;
                case 4:
                    currentVehicle.Doors.GetAll()[2].Open();
                    break;
                case 5:
                    currentVehicle.Doors.GetAll()[3].Open();
                    break;
            }
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

        public async static void Loop()
        {
            // The following if statement handles hiding and unhiding of the vehicle blip as the player enters and exits the vehicle.

            // Enter vehicle
            if(Game.Player.Character.IsInVehicle() && Game.Player.Character.CurrentVehicle == currentVehicle) {
                if (toggleBlip)
                {
                    toggleBlip = !toggleBlip;
                    currentVehicle.AttachedBlip.Delete();
                }
            }
            // Exit vehicle
            else if(!Game.Player.Character.IsInVehicle() && Game.Player.LastVehicle == currentVehicle)
            {
                if (!toggleBlip)
                {
                    toggleBlip = !toggleBlip;
                    currentVehicle.AttachBlip().Sprite = BlipSprite.PersonalVehicleCar;
                    currentVehicle.AttachedBlip.Name = "Personal Vehicle";
                }
            }
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
                // If the vehicle doesn't exist, don't add it to the menu
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
