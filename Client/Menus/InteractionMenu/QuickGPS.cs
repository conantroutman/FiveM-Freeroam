using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.NaturalMotion;
using MenuAPI;
using System.Collections.Generic;
using System.Linq;

namespace Client.Menus.InteractionMenu
{
    class QuickGPS
    {
        private List<string> locationTypesList = new List<string>() { "None", "Mod Shop" };
        private MenuListItem locationTypesListItem;

        private enum locationTypes {
            None = 0,
            ModShop,
        };

        private void CreateItem()
        {
            locationTypesListItem = new MenuListItem("Quick GPS", locationTypesList, 0, "Select to place your waypoint at a set location.");
        }

        public MenuListItem GetItem()
        {
            if (locationTypesListItem == null)
            {
                CreateItem();
            }
            return locationTypesListItem;
        }

        public void SetQuickGPS(int index)
        {
            Vector2 closestLocation;
            switch (index)
            {
                case (int)locationTypes.None:
                    API.DeleteWaypoint();
                    break;
                case (int)locationTypes.ModShop:
                    closestLocation = GetClosestLocation(World.ModShops.GetModShopLocations());
                    API.SetNewWaypoint(closestLocation.X, closestLocation.Y);
                    break;
            }
        }

        private Vector2 GetClosestLocation(List<Vector3> locations)
        {
            Vector3 playerLocation = Game.Player.Character.Position;
            List<float> travelDistances = new List<float>();
            
            // Calculate the travel distance from the the player's location for each of the locations.
            foreach(Vector3 location in locations)
            {
                travelDistances.Add(API.CalculateTravelDistanceBetweenPoints(playerLocation.X, playerLocation.Y, playerLocation.Z, location.X, location.Y, location.Z));
            }

            float shortestDistance = travelDistances.Min();
            Vector3 closestLocation = locations[travelDistances.IndexOf(shortestDistance)];
            return new Vector2(closestLocation.X, closestLocation.Y);
        }
    }
}
