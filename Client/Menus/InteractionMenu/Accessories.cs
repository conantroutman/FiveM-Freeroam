using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using System.Collections.Generic;

namespace Client.Menus.InteractionMenu
{
    class Accessories
    {
        private static Menu menu;

        private List<string> helmetsList = new List<string>() { "None" };
        private MenuListItem helmetsListItem;

        public void CreateMenu()
        {
            menu = new Menu(Game.Player.Name, "Accessories");

            helmetsListItem = new MenuListItem("Helmets", helmetsList, 0);
            menu.AddMenuItem(helmetsListItem);

            // Event handlers
            menu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
            {
                if (listItem == helmetsListItem)
                {
                    API.RemovePedHelmet(Game.Player.Character.Handle, true);
                }
            };
        }

        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }    
    }
}
