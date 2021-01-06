using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.HUD;
using Client.Menus.InteractionMenu;
using Client.UIComponents;

namespace Client.Managers
{
    class HUDManager : BaseScript
    {
        private InteractionMenu InteractionMenu = new InteractionMenu();  // Interaction menu needs to be initialized fter player has spawned
        private BlipController BlipController = new BlipController();
        private Playerlist Playerlist = new Playerlist();
        private Obituaries Obituaries = new Obituaries();
        private GamerTags GamerTags = new GamerTags();
        private readonly Chat Chat = new Chat();

        private static bool isRadarExtended = false;
        private static bool isPlayerListOpen = false;
        private static int radarTimer;
        public HUDManager()
        {
            
        }

        public async void Update()
        {
            //Playerlist.Loop();
            RadarController();
            Chat.Update();
        }

        // Extend the radar by pressing Z
        private async void RadarController()
        {
            if (API.IsControlJustReleased(0, 20))
            {
                if (!isPlayerListOpen && !isRadarExtended)
                {
                    Debug.WriteLine("Opening player list");
                    isPlayerListOpen = !isPlayerListOpen;
                    Playerlist.OpenPlayerList();
                }
                else if (isPlayerListOpen && !isRadarExtended)
                {
                    Debug.WriteLine("Closing player list, opening radar");
                    isPlayerListOpen = !isPlayerListOpen;
                    Playerlist.ClosePlayerList();
                    await BaseScript.Delay(25);
                    ToggleRadarBigMode();
                }
                else if (!isPlayerListOpen && isRadarExtended)
                {
                    Debug.WriteLine("Closing radar");
                    await BaseScript.Delay(25);
                    ToggleRadarBigMode();
                }
            }

            if (isPlayerListOpen)
            {
                Playerlist.RenderPlayerList();
            }

            // Minimize the radar automatically after 10 seconds
            if (isRadarExtended)
            {
                if (API.GetGameTimer() - radarTimer >= 10000)
                {
                    ToggleRadarBigMode();
                }
            }
        }

        private void ToggleRadarBigMode()
        {
            isRadarExtended = !isRadarExtended;
            API.SetRadarBigmapEnabled(isRadarExtended, false);
            radarTimer = API.GetGameTimer();
        }
    }
}
