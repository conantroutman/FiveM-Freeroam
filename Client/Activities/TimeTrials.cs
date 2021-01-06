using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Client.Activities
{
    class TimeTrials
    {
        private static List<TimeTrial> TimeTrialsList = new List<TimeTrial>()
        {
            new TimeTrial(70.1f, new Vector3(-552.626f, 5042.703f, 127.9448f), 0f, new Vector3(500.2789f, 5597.249f, 794.726f), 0f, "collision_99bazvq"),
            new TimeTrial(54.2f, new Vector3(526.397f, 5624.461f, 779.3564f), 0f, new Vector3(2278.745f, 5788.454f, 154.0056f), 0f, "collision_urwqry"),
            new TimeTrial(84.2f, new Vector3(2702.037f, 5145.717f, 42.8568f), 0f, new Vector3(2848.184f, 5945.502f, 355.2424f), 0f, "collision_478rfwn"),
            new TimeTrial(70.1f, new Vector3(-1253.24f, -380.457f, 58.2873f), 0f, new Vector3(-2223.156f, 4254.68f, 45.4055f), 0f, "collision_99bazvl"),
            new TimeTrial(70.1f, new Vector3(-377.166f, 1250.818f, 326.4899f), 0f, new Vector3(2168.589f, 4777.306f, 40.2251f), 0f, "0x8B4ECCC6"),
            new TimeTrial(70.1f, new Vector3(-1502.047f, 4940.611f, 63.8034f), 0f, new Vector3(3782.324f, 4464.408f, 5.0935f), 0f, "0x8B4ECCC6"),
            new TimeTrial(70.1f, new Vector3(1261.353f, -3278.38f, 4.8335f), 0f, new Vector3(95.0126f, 6793.054f, 19.1916f), 0f, "0x8B4ECCC6"),
        };

        private TimeTrial TimeTrial;

        public TimeTrials()
        {
        }
        public async void Update()
        {
            TimeTrialsList.ForEach(timeTrial => {
                timeTrial.DrawMarker();
            });
        }
    }
}
