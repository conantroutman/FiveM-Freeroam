using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.World
{
    class ModShops
    {
        private static List<Vector3> modShopLocations = new List<Vector3>()
        {
            new Vector3(-365.425f, -131.809f, 37.873f),
            new Vector3(-1126.225f, -1993.027f, 0f),
            new Vector3(716.413f, -1078.057f, 0f),
            new Vector3(116.22f, 6606.20f, 31.46f),
            new Vector3(-196.35f, -1303.1f, 30.65f)
        };

        public static List<Vector3> GetModShopLocations()
        {
            return modShopLocations;
        }
    }
}
