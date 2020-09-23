

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
            public Vector3 coords;          //This is the coordinates where the pickup will be created.
            public int hash;             //This is the hash for the pickup type.

            public Pickup(float x, float y, float z, int hash)
            {
                // Offset the coordinates so the pickup isn't clipping through the ground.
                this.coords = new Vector3(x, y, z) + new Vector3(0.1f, 0f, 0.2f);
                this.hash = hash;
            }
        }

        // List of weapon pickups to spawn in the world.
        private static readonly List<Pickup> Pickups = new List<Pickup>()
        {
            // Jerrycans
            new Pickup(207.21f, 6630.832f, 30.7327f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(2663.861f, 3274.759f, 54.2405f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(-2555.263f, 2334.564f, 32.078f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(-724.005f, -937.6594f, 18.0347f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(1737.355f, -1634.247f, 111.4911f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(237.2382f, -2497.145f, 5.5711f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(-1805.078f, 796.3972f, 137.6858f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(592.425f, 2927.029f, 39.9188f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(1693.54f, 4920.416f, 41.0781f, PickupType.WeaponPetrolCan.GetHashCode()),
            new Pickup(2580.882f, 361.7214f, 107.4688f, PickupType.WeaponPetrolCan.GetHashCode()),

            // Knives
            new Pickup(1439.594f, 6331.712f, 22.9575f, PickupType.WeaponKnife.GetHashCode()),
            new Pickup(1531.474f, 3590.724f, 37.7715f, PickupType.WeaponKnife.GetHashCode()),
            new Pickup(-1633.159f, -1053.636f, 3.8816f, PickupType.WeaponKnife.GetHashCode()),
            new Pickup(-60.3809f, -1218.394f, 27.7019f, PickupType.WeaponKnife.GetHashCode()),
            new Pickup(975.5866f, -1810.923f, 30.4854f, PickupType.WeaponKnife.GetHashCode()),

            // Nightsticks
            new Pickup(850.1597f, -1317.404f, 25.4525f, PickupType.WeaponNightstick.GetHashCode()),
            new Pickup(-442.715f, 5996.535f, 30.4901f, PickupType.WeaponNightstick.GetHashCode()),
            new Pickup(-1104.75f, -831.6023f, 36.6754f, PickupType.WeaponNightstick.GetHashCode()),
            new Pickup(1838.486f, 3691.788f, 33.267f, PickupType.WeaponNightstick.GetHashCode()),
            new Pickup(1831.211f, 2602.11f, 44.6046f, PickupType.WeaponNightstick.GetHashCode()),

            // Hammers
            new Pickup(-813.6743f, -798.7653f, 18.5765f, API.GetHashKey("PICKUP_WEAPON_HAMMER")),    // Hammer enum seems to be missing?
            new Pickup(-338.8805f, 6295.073f, 34.2573f, API.GetHashKey("PICKUP_WEAPON_HAMMER")),
            new Pickup(2675.516f, 3500.448f, 52.3f, API.GetHashKey("PICKUP_WEAPON_HAMMER")),
            new Pickup(-82.1495f, 935.0021f, 232.0286f, API.GetHashKey("PICKUP_WEAPON_HAMMER")),
            new Pickup(120.4866f, -3182.438f, 4.9906f, API.GetHashKey("PICKUP_WEAPON_HAMMER")),

            // Bats
            new Pickup(-317.6021f, -1647.413f, 30.8532f, PickupType.WeaponBat.GetHashCode()),
            new Pickup(-359.9102f, -1866.588f, 19.52816f, PickupType.WeaponBat.GetHashCode()),
            new Pickup(-1755.525f, 185.8265f, 63.4436f, PickupType.WeaponBat.GetHashCode()),
            new Pickup(1736.144f, 6419.344f, 34.0372f, PickupType.WeaponBat.GetHashCode()),
            new Pickup(245.6425f, 3166.322f, 41.8399f, PickupType.WeaponBat.GetHashCode()),

            // Crowbars
            new Pickup(-553.0112f, 5325.966f, 72.5996f, PickupType.WeaponCrowbar.GetHashCode()),
            new Pickup(-1810.52f, 3104.278f, 31.8418f, PickupType.WeaponCrowbar.GetHashCode()),
            new Pickup(-2947.52f, 438.6648f, 14.2617f, PickupType.WeaponCrowbar.GetHashCode()),
            new Pickup(1346.287f, 4390.351f, 43.3438f, PickupType.WeaponCrowbar.GetHashCode()),
            new Pickup(1238.739f, -2969.334f, 8.3193f, PickupType.WeaponCrowbar.GetHashCode()),

            // Golf clubs
            new Pickup(-1602.416f, 3.309f, 59.9076f, PickupType.WeaponGolfclub.GetHashCode()),
            new Pickup(-1341.042f, 48.6275f, 54.2456f, PickupType.WeaponGolfclub.GetHashCode()),
            new Pickup(1330.463f, -608.6297f, 73.5131f, PickupType.WeaponGolfclub.GetHashCode()),
            new Pickup(-1029.947f, -2737.736f, 19.1693f, PickupType.WeaponGolfclub.GetHashCode()),
            new Pickup(-2167.089f, 5197.074f, 15.8854f, PickupType.WeaponGolfclub.GetHashCode()),

            // Bottles
            new Pickup(2526.463f, 2583.651f, 36.9449f, PickupType.WeaponBottle.GetHashCode()),
            new Pickup(1073.333f, -260.834f, 58.0789f, PickupType.WeaponBottle.GetHashCode()),
            new Pickup(63.1339f, 3671.724f, 38.8075f, PickupType.WeaponBottle.GetHashCode()),
            new Pickup(-131.3041f, 6378.745f, 31.18f, PickupType.WeaponBottle.GetHashCode()),
            new Pickup(2220.467f, 5611.511f, 53.6791f, PickupType.WeaponBottle.GetHashCode()),

            // Daggers
            new Pickup(2955.139f, 3134.444f, 170.5797f, API.GetHashKey("PICKUP_WEAPON_DAGGER")),    // Dagger enum seems to be missing?
            new Pickup(-572.4406f, -613.0344f, 29.4478f, API.GetHashKey("PICKUP_WEAPON_DAGGER")),
            new Pickup(-446.4161f, 2013.069f, 122.5453f, API.GetHashKey("PICKUP_WEAPON_DAGGER")),

            // Grenades
            new Pickup(-2055.09f, 3238.867f, 30.4989f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(-1937.804f, 2051.439f, 139.8329f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(713.5492f, 4093.677f, 33.7329f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(1525.571f, 1710.302f, 109.007f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(2815.865f, -668.2856f, 0.1915f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(1024.388f, 134.75f, 89.2405f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(-444.1653f, -892.3921f, 46.9889f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(1255.207f, -1571.801f, 77.9696f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(-1.6907f, -1822.665f, 28.5482f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(-1065.103f, -1163.269f, 1.1586f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(-1002.326f, 729.7162f, 163.0705f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(-102.7766f, 2806.671f, 52.0369f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(2491.141f, 4966.585f, 43.6179f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(1362.785f, 6549.668f, 13.5391f, PickupType.WeaponGrenade.GetHashCode()),
            new Pickup(-838.8696f, 4186.138f, 214.295f, PickupType.WeaponGrenade.GetHashCode()),

            // Molotovs
            new Pickup(-1101.057f, 2723.037f, 17.8004f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(1598.074f, 3586.79f, 37.7715f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(88.6023f, 3743.183f, 39.7783f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(2318.18f, 2551.38f, 46.6955f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(-1598.31f, 5188.995f, 3.3151f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(1321.452f, 4307.717f, 37.0253f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(3613.571f, 5025.491f, 10.3525f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(256.3964f, -1109.345f, 28.7002f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(1419.453f, -2604.119f, 46.9843f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(1330.674f, -1660.243f, 50.2364f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(-3262.536f, 960.1833f, 7.3522f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(658.4307f, 1278.831f, 359.296f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(664.42f, 551.9471f, 128.4458f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(147.3479f, 6865.898f, 27.3152f, PickupType.WeaponMolotov.GetHashCode()),
            new Pickup(-174.7206f, 297.1453f, 92.775f, PickupType.WeaponMolotov.GetHashCode()),

            // Sticky bombs
            new Pickup(-2141.6f, 3312.421f, 37.7325f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(734.3641f, 2592.939f, 72.8021f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(1662.068f, -26.2036f, 172.7748f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(-2295.89f, 198.9769f, 166.6f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(2945.487f, 2746.661f, 42.3869f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(1559.965f, -2165.721f, 76.4832f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(1297.402f, -3349.205f, 4.9016f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(-153.1925f, -1099.046f, 12.117f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(-1911.846f, -3032.723f, 22.5878f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(-465.1981f, -2273.874f, 7.5208f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(85.2738f, -436.2608f, 35.0055f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(-263.0924f, 4729.493f, 137.3357f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(2809.978f, 5985.195f, 349.6869f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(-1548.144f, -272.6125f, 45.7184f, PickupType.WeaponStickyBomb.GetHashCode()),
            new Pickup(-438.7823f, 1597.313f, 357.4732f, PickupType.WeaponStickyBomb.GetHashCode()),

            // Smoke grenades
            new Pickup(433.8869f, -994.8528f, 24.7952f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(870.1877f, -1347.616f, 25.3143f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(402.9637f, -1626.409f, 28.2919f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(-964.0244f, -2798.867f, 13.257f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(-1029.41f, -843.2015f, 9.8502f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(-319.5805f, 6085.437f, 30.4459f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(3426.096f, 3761.877f, 29.6426f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(2661.03f, 1642.761f, 23.874f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(9.0143f, -2531.992f, 5.1515f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(639.5297f, 11.7998f, 81.7956f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(-1706.16f, 187.5831f, 62.926f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(-563.0706f, -134.6288f, 37.0794f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(831.0158f, -2195.54f, 29.2622f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(485.1099f, -3112.01f, 5.2944f, PickupType.WeaponSmokeGrenade.GetHashCode()),
            new Pickup(-1327.578f, -1520.638f, 3.4322f, PickupType.WeaponSmokeGrenade.GetHashCode()),

            // Proximity mines
            new Pickup(574.8237f, -3126.324f, 17.7686f, API.GetHashKey("PICKUP_WEAPON_PROXMINE")),
            new Pickup(-2349.4f, 3267.049f, 31.8107f, API.GetHashKey("PICKUP_WEAPON_PROXMINE")),

            // Combat pistols
            new Pickup(-1592.5f, 2796.747f, 15.9301f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(-2646.445f, 1874.011f, 159.134f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(1665.361f, 1.32f, 165.1181f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(-1504.432f, -39.491f, 53.5484f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(-1086.262f, -2403.724f, 12.9452f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(4.0018f, -1215.049f, 25.708f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(726.014f, -749.1634f, 24.6943f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(785.92f, 2165.659f, 52.0979f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(1717.843f, 4680.056f, 42.6558f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(-366.4723f, 6336.608f, 28.8537f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(-1230.751f, -922.8118f, 1.1502f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(789.6778f, -3167.869f, 4.9941f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(261.5213f, 204.2065f, 109.2873f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(-1051.675f, -477.2152f, 35.9871f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(2577.379f, -295.4593f, 92.0782f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(1271.555f, -1706.646f, 53.655f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(259.4435f, -1357.18f, 29.5518f, PickupType.WeaponCombatPistol.GetHashCode()),
            new Pickup(-783.2446f, 187.8259f, 71.8353f, PickupType.WeaponCombatPistol.GetHashCode()),

            // Micro SMGs
            new Pickup(1532.304f, 3796.368f, 32.5178f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(-387.8509f, 1134.989f, 321.6291f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(3313.125f, 5177.851f, 18.6196f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(156.7099f, 3130.233f, 42.5891f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(-311.3634f, -1199.772f, 23.7682f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(304.044f, -3250.736f, 4.8007f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(115.3294f, -1973.73f, 19.9264f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(-1072.865f, -1675.74f, 3.5128f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(-926.1714f, -2939.181f, 12.9451f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(-105.4509f, 0.7935f, 77.4424f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(-1068.803f, 4893.899f, 213.2765f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(2289.758f, 1719.285f, 67.0406f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(455.9319f, 5572.014f, 780.1841f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(753.1926f, 6463.068f, 30.0616f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(1902.835f, 613.0169f, 189.4283f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(1861.552f, -1105.026f, 83.2897f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(-1513.446f, 1524.123f, 110.6261f, PickupType.WeaponMicroSMG.GetHashCode()),
            new Pickup(-1616.83f, 762.6611f, 188.2431f, PickupType.WeaponMicroSMG.GetHashCode()),

            // Assault rifles
            new Pickup(-1147.023f, 4939.17f, 221.2736f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(-2788.43f, 1418.052f, 99.907f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(1406.931f, 1138.057f, 113.5431f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(-263.2222f, 2196.078f, 129.4037f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(-1757.354f, 427.6137f, 126.685f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(1942.285f, 4657.152f, 39.5475f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(1133.095f, -2263.399f, 30.9256f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(2852.433f, -1347.092f, 14.9853f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(-282.8943f, -2780.49f, 3.372f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(2339.248f, 3124.61f, 47.2087f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(902.6002f, 3565.873f, 32.7947f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(3825.333f, 4439.726f, 1.8011f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(-569.1706f, -1632.271f, 18.4124f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(-13.0164f, 6668.36f, 30.9201f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(-35.289f, 820.8724f, 230.3325f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(73.9799f, -877.1083f, 29.4405f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(983.5856f, -102.6713f, 73.8538f, PickupType.WeaponAssaultRifle.GetHashCode()),
            new Pickup(3063.959f, 2219.765f, 2.0363f, PickupType.WeaponAssaultRifle.GetHashCode()),

            // Pump shotguns
            new Pickup(2943.024f, 4625.229f, 47.7259f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(731.0479f, 2530.424f, 72.2271f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(56.8977f, 3690.629f, 38.9213f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(1486.623f, 1131.212f, 113.3367f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(2354.291f, 2610.553f, 45.6676f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(-678.2736f, 5792.836f, 16.331f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(-2868.661f, -12.333f, 10.6082f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(161.8364f, -561.1632f, 20.9956f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(1280.353f, 304.2236f, 80.9909f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(-60.6782f, -1440.506f, 31.1225f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(1581.334f, 2910.53f, 55.9464f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(-2172.474f, 4293.438f, 48.0238f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(742.8358f, 4170.15f, 40.0929f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(1591.988f, 6581.986f, 12.9679f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(-1890.422f, 2073.835f, 139.9977f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(944.5846f, -678.1459f, 57.4548f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(-666.053f, -2003.23f, 6.5173f, PickupType.WeaponPumpShotgun.GetHashCode()),
            new Pickup(2727.292f, 4143.927f, 43.2929f, PickupType.WeaponPumpShotgun.GetHashCode()),

            // Sniper rifles
            new Pickup(1989.886f, 5026.946f, 60.6082f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(2729.045f, 1577.512f, 65.5431f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(-2507.8f, 3299.061f, 90.969f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(1815.508f, 3906.426f, 36.2175f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(424.9106f, 5614.028f, 765.529f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(1004.394f, -2880.216f, 38.1619f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(352.8205f, 170.5852f, 126.7566f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(-1272.359f, -2444.987f, 72.0491f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(-1175.81f, -472.4921f, 59.1132f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(1611.97f, -2245.351f, 131.7942f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(763.5873f, 1185.768f, 348.0852f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(-275.6897f, -636.3749f, 47.4426f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(-2041.719f, -373.5576f, 47.1062f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(-521.8686f, 4196.347f, 192.736f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(529.0732f, -2358.559f, 48.9981f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(172.0419f, 2220.292f, 89.7872f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(25.5916f, 7644.835f, 17.9513f, PickupType.WeaponSniperRifle.GetHashCode()),
            new Pickup(70.5065f, 1224.164f, 272.0053f, PickupType.WeaponSniperRifle.GetHashCode()),
        };

        // Create the pickups in the world.
        public static void CreatePickups()
        {
            Pickups.ForEach((_pickup) =>
            {
                // Weapons spawn with 2 mags
                pickupAmmo = API.GetMaxAmmoInClip(API.PlayerPedId(), (uint)API.GetWeaponTypeFromPickupType(_pickup.hash), true) * 2;
                weaponPickup = API.CreatePickupRotate((uint)_pickup.hash, _pickup.coords.X, _pickup.coords.Y, _pickup.coords.Z, 0f, 360f, 0f, 512, pickupAmmo, 3, false, 0);
                API.SetPickupRegenerationTime(weaponPickup, 10000);
            });
        }
    }
}
