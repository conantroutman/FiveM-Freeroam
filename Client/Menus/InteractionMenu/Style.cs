using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Menus.InteractionMenu
{
    class Style
    {
        private Menu Menu;
        private Accessories Accessories;

        private static MenuListItem toggleMotorcycleHelmetOptionsItem;
        private List<string> toggleMotorcycleHelmetOptions = new List<string>() { "On", "Off" };
        private bool toggleMotorcycleHelmet = true;

        private void CreateMenu()
        {
            Menu = new Menu(Game.Player.Name, "Style");
            Accessories = new Accessories();

            MenuController.AddSubmenu(Menu, Accessories.GetMenu());
            MenuItem AccessoriesItem = new MenuItem("Accessories", "Contains options that allow you to change your parachute appearance.");
            Menu.AddMenuItem(AccessoriesItem);
            MenuController.BindMenuItem(Menu, Accessories.GetMenu(), AccessoriesItem);

            toggleMotorcycleHelmetOptionsItem = new MenuListItem("Auto Show Bike Helmet", toggleMotorcycleHelmetOptions, 0);
            Menu.AddMenuItem(toggleMotorcycleHelmetOptionsItem);

            Menu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
            {
                if (listItem == toggleMotorcycleHelmetOptionsItem)
                {
                    API.SetPedConfigFlag(Game.Player.Character.Handle, 35, !Convert.ToBoolean(listIndex));
                }
            };
        }

        public Menu GetMenu()
        {
            if (Menu == null)
            {
                CreateMenu();
            }
            return Menu;
        }

        private void ToggleMotorcycleHelmet()
        {
            toggleMotorcycleHelmet = !toggleMotorcycleHelmet;
            
        }
    }
}
