using CitizenFX.Core;
using MenuAPI;

namespace Client.Menus.InteractionMenu
{
    class InteractionMenu
    {
        private static Menu Menu { get; set; }
        private static QuickGPS QuickGPSItem;
        private static Inventory Inventory;
        private static Style Style;

        public void CreateMenu()
        {
            Menu = new Menu(Game.Player.Name, "Main Menu");
            MenuController.AddMenu(Menu);
            MenuController.MainMenu = Menu;

            CreateMenuItems();
        }

        private void CreateMenuItems()
        {
            QuickGPSItem = new QuickGPS();
            Menu.AddMenuItem(QuickGPSItem.GetItem());

            Menu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
            {
                if (listItem == QuickGPSItem.GetItem())
                {
                    QuickGPSItem.SetQuickGPS(listIndex);
                }
            };

            Inventory = new Inventory();
            MenuController.AddSubmenu(Menu, Inventory.GetMenu());
            MenuItem InventoryButton = new MenuItem("Inventory", "Your Inventory contains carried items such as weapon ammo and snacks.");
            Menu.AddMenuItem(InventoryButton);
            MenuController.BindMenuItem(Menu, Inventory.GetMenu(), InventoryButton);

            Style = new Style();
            MenuController.AddSubmenu(Menu, Style.GetMenu());
            MenuItem StyleButton = new MenuItem("Style", "View and change player options.");
            Menu.AddMenuItem(StyleButton);
            MenuController.BindMenuItem(Menu, Style.GetMenu(), StyleButton);
        }
    }
}
