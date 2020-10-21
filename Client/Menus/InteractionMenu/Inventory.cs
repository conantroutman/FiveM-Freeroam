using CitizenFX.Core;
using MenuAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Menus.InteractionMenu
{
    class Inventory
    {
        private Menu Menu;
        private Accessories Accessories;

        public Inventory()
        {
            Menu = new Menu(Game.Player.Name, "Inventory");
            Accessories = new Accessories();

            MenuController.AddSubmenu(Menu, Accessories.GetMenu());
            MenuItem AccesoriesItem = new MenuItem("Accessories");
        }
        private void CreateMenu()
        {
            
        }

        public Menu GetMenu()
        {
            if (Menu == null)
            {
                CreateMenu();
            }
            return Menu;
        }
    }
}
