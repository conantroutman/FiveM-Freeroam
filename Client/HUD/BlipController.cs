using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Client.Players;
using System;

namespace Client.HUD
{
    class BlipController : BaseScript
    {
        private static Blip blip;
        private static int vehicle;
        private static PlayerList playerList;
        private static bool isBlipRotating = false;

        public BlipController()
        {
            playerList = new PlayerList();

            CreatePlayerBlips();
        }
        private enum VehicleBlips
        {
            
        }

        private void CreatePlayerBlips()
        {
            foreach (Player player in playerList)
            {
                if (player != Game.Player && !API.DoesBlipExist(API.GetBlipFromEntity(player.Handle)))
                {
                    CreatePlayerBlip(player);
                }
            }
        }

        private void CreatePlayerBlip(Player player)
        {
            blip = player.Character.AttachBlip();
            API.SetBlipCategory(blip.Handle, 7);
            blip.Name = player.Name;
            API.SetBlipColour(blip.Handle, Colors.GetColor(player) - 22);
            blip.IsShortRange = true;
        }

        /*public async void Update()
        {
            foreach(Player player in players)
            {
                blip = API.GetBlipFromEntity(player.Handle);
                if(player.Character.IsDead)
                {
                    SetBlip(player, blip, (int)BlipSprite.Dead, false, false);
                } else
                {
                    if (player.Character.IsInVehicle())
                    {
                        switch(player.Character.CurrentVehicle.ClassType)
                        {
                            case VehicleClass.Helicopters:
                                SetBlip(player, blip, (int)BlipSprite.HelicopterAnimated, false, false);
                                break;
                            case VehicleClass.Planes:
                                SetBlip(player, blip, (int)BlipSprite.Plane, false, true);
                                break;
                            case VehicleClass.Boats:
                                SetBlip(player, blip, 427, false, true);
                                break;
                        }
                    } else
                    {
                        SetBlip(player, blip, 1, true, false);
                    }
                }

                if (isBlipRotating)
                {
                    API.SetBlipRotation(blip, (int)Math.Ceiling(API.GetEntityHeading(player.Character.Handle)));
                }
            }
        }*/

        private static void SetBlip(Player player, int blip, int sprite, bool showHeading, bool isRotating)
        {
            if (API.GetBlipSprite(blip) != sprite)
            {
                API.SetBlipSprite(blip, sprite);
                API.SetBlipColour(blip, Colors.GetColor(Game.Player) - 22);
                API.ShowHeadingIndicatorOnBlip(blip, showHeading);
                API.SetBlipNameToPlayerName(blip, player.Handle);
            }
            isBlipRotating = isRotating;
        }

        /*public static void Create()
        {
            players = new PlayerList();
            foreach(Player player in players)
            {
                if (!API.DoesBlipExist(blip))
                {
                    blip = API.AddBlipForEntity(player.Character.Handle);
                    SetBlip(player, blip, 1, true, false);
                    API.SetBlipCategory(blip, 7);
                }
            }
        }*/

        [EventHandler("newPlayerConnected")]
        private void newPlayerConnected(Player connectedPlayer)
        {
            playerList = new PlayerList();
            CreatePlayerBlips();
            Screen.ShowNotification($"{connectedPlayer.Name} connected.");
        }

        [EventHandler("playerDisconnected")]
        private void playerDisconnected(Player disconnectedPlayer)
        {
            playerList = new PlayerList();
            Screen.ShowNotification($"{disconnectedPlayer.Name} disconnected.");
        }
    }
}
