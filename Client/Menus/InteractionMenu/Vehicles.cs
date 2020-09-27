using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Gameplay;
using MenuAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Menus.InteractionMenu
{
    class Vehicles : BaseScript
    {
        private static Menu menu;
        private static Menu requestVehicleMenu;
        private static MenuItem requestVehicleButton;

        private static List<string> doorsList = new List<string>()
        {
            "None",
            "All",
            "Left Door",
            "Right Door",
            "Hood",
            "Trunk"
        };
        private static MenuListItem doorsItem;

        private static Menu remoteControlsMenu;
        private static MenuItem remoteControlsMenuButton;

        private static Vehicle currentVehicle;
        private static PersonalVehicleController vehicleController;

        public Vehicles()
        {
            vehicleController = MainClient.PersonalVehicleController;
        }
        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }
        public static void CreateMenu()
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
            remoteControlsMenuButton = new MenuItem("Vehicle Remote Functions", "Select to adjust vehicle engine, lights and radio options" );
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
                    API.SetVehicleEngineOn(vehicleController.currentVehicle.Handle, listIndex == 1, true, false);
                }
                else if (listItem == toggleHeadlightsItem)
                {
                    vehicleController.currentVehicle.AreLightsOn = listIndex == 1;
                }
                else if (listItem == toggleNeonItem)
                {
                    API.SetVehicleNeonLightEnabled(vehicleController.currentVehicle.Handle, 0, listIndex == 1);
                    API.SetVehicleNeonLightEnabled(vehicleController.currentVehicle.Handle, 1, listIndex == 1);
                    API.SetVehicleNeonLightEnabled(vehicleController.currentVehicle.Handle, 2, listIndex == 1);
                    API.SetVehicleNeonLightEnabled(vehicleController.currentVehicle.Handle, 3, listIndex == 1);
                }
                else if (listItem == toggleRadioItem)
                {
                    API.SetVehicleRadioEnabled(vehicleController.currentVehicle.Handle, listIndex == 1);
                    API.SetVehRadioStation(vehicleController.currentVehicle.Handle, "RADIO_04_PUNK");
                    API.SetVehicleRadioLoud(vehicleController.currentVehicle.Handle, listIndex == 1);
                }
            };
        }

        //--------------------------------------------------------------------
        // CreateRequestVehicleMenu - Creates a submenu for spawning vehicles
        //--------------------------------------------------------------------
        private static void CreateRequestVehicleMenu()
        {
            requestVehicleMenu = new Menu(Game.Player.Name, "Request Vehicle");
            MenuController.AddSubmenu(menu, requestVehicleMenu);
            requestVehicleButton = new MenuItem("Request Vehicle", "Get the mechanic to deliver a vehicle of your choice.");
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
        private static void CreateClassMenus(string title, List<string> vehiclesList)
        {
            VehicleClassMenu classMenu = new VehicleClassMenu(title, vehiclesList);

            MenuController.AddSubmenu(requestVehicleMenu, classMenu.GetMenu());
            MenuItem classMenuButton = new MenuItem(title, "Fuck");
            requestVehicleMenu.AddMenuItem(classMenuButton);
            MenuController.BindMenuItem(requestVehicleMenu, classMenu.GetMenu(), classMenuButton);
        }

        private static void DoorControls(int index)
        {
            switch (index)
            {
                case 0:
                    foreach (VehicleDoor door in vehicleController.currentVehicle.Doors.GetAll())
                    {
                        door.Close();
                    }
                    break;
                case 1:
                    foreach (VehicleDoor door in vehicleController.currentVehicle.Doors.GetAll())
                    {
                        door.Open();
                    }
                    break;
                case 2:
                    vehicleController.currentVehicle.Doors.GetAll()[0].Open();
                    break;
                case 3:
                    vehicleController.currentVehicle.Doors.GetAll()[1].Open();
                    break;
                case 4:
                    vehicleController.currentVehicle.Doors.GetAll()[2].Open();
                    break;
                case 5:
                    vehicleController.currentVehicle.Doors.GetAll()[3].Open();
                    break;
            }
        }

        public static void RequestVehicle(string modelName)
        {
            vehicleController.SummonVehicle(modelName);
        }

        [EventHandler("enteredPersonalVehicle")]
        private void enteredPersonalVehicle()
        {
            requestVehicleButton.Enabled = false;
            doorsItem.Enabled = false;
            remoteControlsMenuButton.Enabled = false;
        }

        [EventHandler("exitedPersonalVehicle")]
        private void exitedPersonalVehicle()
        {
            requestVehicleButton.Enabled = true;
            doorsItem.Enabled = true;
            remoteControlsMenuButton.Enabled = true;
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
                Vehicles.RequestVehicle(vehicleItemList[index].ItemData);
            };
        }

        public Menu GetMenu()
        {
            return menu;
        }
    }
}
