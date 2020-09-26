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
        private List<string> locationTypesList = new List<string>() { "None", "Mod Shop", "Airport", "Military Base", "Arena", "Golf Club" };
        private MenuListItem locationTypesListItem;

        private enum locationTypes {
            None = 0,
            ModShop,
            Airport,
            MilitaryBase,
            Arena,
            GolfClub
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
                    closestLocation = GetClosestLocation(WorldContent.ModShops.GetModShopLocations());
                    API.SetNewWaypoint(closestLocation.X, closestLocation.Y);
                    break;
                case (int)locationTypes.Airport:
                    API.SetNewWaypoint(1097.386f, -3016.153f);
                    break;
                case (int)locationTypes.MilitaryBase:
                    API.SetNewWaypoint(-2263.075f, 3150.41f);
                    break;
                case (int)locationTypes.Arena:
                    API.SetNewWaypoint(-185.174f, -2008.624f);
                    break;
                case (int)locationTypes.GolfClub:
                    API.SetNewWaypoint(-1376.666f, 56.507f);
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
