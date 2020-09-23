

using CitizenFX.Core.Native;
using CitizenFX.Core;
using System.Collections.Generic;

namespace Client.World
{
    class WeaponPickups
    {
        private static int weaponPickup;
        private static int pickupAmmo;
        private struct Pickup
        {
            public Vector3 coords;
            public string hash;

            public Pickup(float x, float y, float z, string hash)
            {
                this.coords = new Vector3(x, y, z) + new Vector3(0.1f, 0f, 0.2f);
                this.hash = hash;
            }
        }

        // List of weapon pickups to spawn in the world.
        private static readonly List<Pickup> Pickups = new List<Pickup>()
        {
            // Jerrycans
            new Pickup(207.21f, 6630.832f, 30.7327f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(2663.861f, 3274.759f, 54.2405f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(-2555.263f, 2334.564f, 32.078f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(-724.005f, -937.6594f, 18.0347f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(1737.355f, -1634.247f, 111.4911f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(237.2382f, -2497.145f, 5.5711f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(-1805.078f, 796.3972f, 137.6858f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(592.425f, 2927.029f, 39.9188f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(1693.54f, 4920.416f, 41.0781f, "PICKUP_WEAPON_PETROLCAN"),
            new Pickup(2580.882f, 361.7214f, 107.4688f, "PICKUP_WEAPON_PETROLCAN"),

            // Knives
            new Pickup(1439.594f, 6331.712f, 22.9575f, "PICKUP_WEAPON_KNIFE"),
            new Pickup(1531.474f, 3590.724f, 37.7715f, "PICKUP_WEAPON_KNIFE"),
            new Pickup(-1633.159f, -1053.636f, 3.8816f, "PICKUP_WEAPON_KNIFE"),
            new Pickup(-60.3809f, -1218.394f, 27.7019f, "PICKUP_WEAPON_KNIFE"),
            new Pickup(975.5866f, -1810.923f, 30.4854f, "PICKUP_WEAPON_KNIFE"),
        };

        public static void CreatePickups()
        {
            Pickups.ForEach((_pickup) =>
            {
                pickupAmmo = API.GetMaxAmmoInClip(API.PlayerPedId(), (uint)API.GetWeaponTypeFromPickupType(API.GetHashKey(_pickup.hash)), true) * 2;
                weaponPickup = API.CreatePickupRotate((uint)API.GetHashKey(_pickup.hash), _pickup.coords.X, _pickup.coords.Y, _pickup.coords.Z, 0f, 360f, 0f, 512, pickupAmmo, 3, false, 0);
                API.SetPickupRegenerationTime(weaponPickup, 10000);
            });
        }
    }
}
