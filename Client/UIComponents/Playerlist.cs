using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// C# port of https://rage.mp/files/file/333-player-list/
namespace Client.UIComponents
{
    class Playerlist
    {
        private Scaleform cardScaleform;

        private int DEFAULT_PAGE = 1;
        private int PLAYERS_PER_PAGE = 16;

        private string CARD_SOUNDSET_NAME = "HUD_FRONTEND_DEFAULT_SOUNDSET";
        private string OPEN_SOUND_NAME = "LEADER_BOARD";
        private string CLOSE_SOUND_NAME = "BACK";
        private string NAV_SOUND_NAME = "NAV_UP_DOWN";

        private bool playerListOpen = false;
        private int playerListCurPage = 1;
        private int playerListMaxPage = 0;
        private PlayerList playerList;

        private List<string> playerHeadshots = new List<string>();

        private List<Player> Paginate(PlayerList list, int pageSize, int pageNumber)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageNumber * pageSize).ToList();
        }

        private void UpdateTitle()
        {
            if (cardScaleform == null)
            {
                return;
            }

            cardScaleform.CallFunction("SET_TITLE", $"FiveM ({playerList.Count()})", $"Page {playerListCurPage}/{playerListMaxPage}");
        }

        private void UpdateCard()
        {
            if (cardScaleform == null)
            {
                return;
            }

            // Reset all indices first
            for (int i = 0; i < PLAYERS_PER_PAGE; i++)
            {
                cardScaleform.CallFunction("SET_DATA_SLOT_EMPTY", i);
            }

            List<Player> players = Paginate(playerList, PLAYERS_PER_PAGE, playerListCurPage);
            foreach(Player player in players)
            {
                int index = players.IndexOf(player);
                int color = Players.Colors.GetColor(player);
                string headshot = playerHeadshots[index];

                cardScaleform.CallFunction("SET_DATA_SLOT", index, "", $"{SanitizeName(player.Name)}", color, 0, "", "", "", 2, headshot, headshot);
            }

            cardScaleform.CallFunction("DISPLAY_VIEW");
        }

        private string SanitizeName(string name)
        {
            //return Regex.Replace(name, "/[<>~]/g", "");
            return name;
        }

        private async Task GetPlayerHeadshots(PlayerList list)
        {
            int headshotHandle;
            string headshotTxd;

            foreach (Player player in list)
            {
                headshotHandle = API.RegisterPedheadshot(player.Character.Handle);
                while (!API.IsPedheadshotReady(headshotHandle)) { await BaseScript.Delay(0); }
                headshotTxd = API.GetPedheadshotTxdString(headshotHandle);
                playerHeadshots.Add(headshotTxd);
                API.UnregisterPedheadshot(headshotHandle);
            }
        }

        public async void Loop()
        {
            if (Game.IsControlJustPressed(0, Control.MultiplayerInfo))
            {
                
                playerListCurPage = DEFAULT_PAGE;

                if (playerListOpen)
                {
                    Debug.WriteLine("Close Player List");
                    if (cardScaleform != null)
                    {
                        cardScaleform.Dispose();
                        cardScaleform = null;
                    }

                    Audio.PlaySoundFrontend(CLOSE_SOUND_NAME);
                }
                else
                {
                    Debug.WriteLine("Open Player List");
                    playerList = new PlayerList();
                    await GetPlayerHeadshots(playerList);
                    playerListMaxPage = (int)Math.Ceiling((double)playerList.Count() / PLAYERS_PER_PAGE);

                    cardScaleform = new Scaleform("mp_mm_card_freemode");
                    while (!cardScaleform.IsLoaded) { await BaseScript.Delay(0); }
                    Debug.WriteLine($"IsScaleformLoaded: {cardScaleform}");

                    UpdateTitle();
                    UpdateCard();

                    Audio.PlaySoundFrontend(OPEN_SOUND_NAME);
                }

                playerListOpen = !playerListOpen;
            }

            if (playerListOpen)
            {
                API.SetScriptGfxAlign(76, 84);
                API.DrawScaleformMovie(cardScaleform.Handle, 0.122f, 0.3f, 0.28f, 0.6f, 255, 255, 255, 255, 0);
                API.ResetScriptGfxAlign();
            }
        }
    }
}
