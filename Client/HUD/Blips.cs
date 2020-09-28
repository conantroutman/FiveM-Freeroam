using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;

namespace Client.HUD
{
    class Blips
    {
        private static int blip;
        private static int vehicle;
        private static PlayerList players;
        private static bool isBlipRotating = false;
        private enum VehicleBlips
        {
            
        }

        public async static void Update()
        {
            foreach(Player player in players)
            {
                blip = player.Character.AttachedBlip.Handle;
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
        }

        private static void SetBlip(Player player, int blip, int sprite, bool showHeading, bool isRotating)
        {
            if (API.GetBlipSprite(blip) != sprite)
            {
                API.SetBlipSprite(blip, sprite);
                API.SetBlipColour(blip, Players.Colors.GetColor(Game.Player) - 22);
                API.ShowHeadingIndicatorOnBlip(blip, showHeading);
                API.SetBlipNameToPlayerName(blip, player.Handle);
            }
            isBlipRotating = isRotating;
        }

        public static void Create()
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
        }
    }
}
