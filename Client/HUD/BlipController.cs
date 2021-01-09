using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Client.Players;
using System;

namespace Client.HUD
{
    class BlipController : BaseScript
    {
        private static PlayerList playerList;
        private bool doesBlipExist = false;

        public BlipController()
        {
            playerList = new PlayerList();

            //Initialize blips
            //InitBlips();

            EventHandlers["firstSpawn"] += new Action(InitBlips);
            EventHandlers["playerRespawned"] += new Action<int>(CreatePlayerBlip);
            EventHandlers["playerLeft"] += new Action<int>(RemovePlayerBlip);
            EventHandlers["playerKilled"] += new Action<int>(SetPlayerBlipDead);
            EventHandlers["playerDied"] += new Action<int>(SetPlayerBlipDead);
            EventHandlers["playerEnteredVehicle"] += new Action<int, string>(SetPlayerBlipVehicle);
            EventHandlers["playerLeftVehicle"] += new Action<int>(CreatePlayerBlip);
        }

        private void InitBlips()
        {
            foreach(Player player in playerList)
            {
                Debug.WriteLine($"Found player {player.Name}");
                CreatePlayerBlip(player.ServerId);
                if (player.Character.IsInVehicle())
                {
                    //SetPlayerBlipVehicle(player.ServerId);
                }
            }
        }

        private void CreatePlayerBlip(int serverId)
        {
            Player player = Players[serverId];
            if (player.Handle != Game.Player.Handle)
            {
                if(API.DoesEntityExist(player.Character.Handle) && !API.DoesBlipExist(API.GetBlipFromEntity(player.Character.Handle)))
                {
                    Blip blip = player.Character.AttachBlip();
                    API.SetBlipCategory(blip.Handle, 7);
                    blip.Name = player.Name;
                    API.SetBlipColour(blip.Handle, (5 + player.ServerId));
                    API.ShowHeadingIndicatorOnBlip(blip.Handle, true);
                    blip.IsShortRange = true;
                } else if (API.DoesEntityExist(player.Character.Handle))
                {
                    SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 1, true, false);
                }
            }
        }

        private static void SetBlip(Player player, int blip, int sprite, bool showHeading, bool isRotating)
        {
            if (API.GetBlipSprite(blip) != sprite)
            {
                API.SetBlipSprite(blip, sprite);
                API.SetBlipColour(blip, 5 + player.ServerId);
                API.ShowHeadingIndicatorOnBlip(blip, showHeading);
                API.SetBlipNameToPlayerName(blip, player.Handle);
            }
            bool isBlipRotating = isRotating;
        }

        private void SetPlayerBlipDead(int serverId)
        {
            Player player = Players[serverId];

            int deadPlayerBlip = API.GetBlipFromEntity(player.Character.Handle);
            if (API.DoesBlipExist(deadPlayerBlip))
            {
                SetBlip(player, deadPlayerBlip, 274, false, false);
            }
        }

        private void SetPlayerBlipVehicle(int serverId, string vehicleName)
        {
            Player player = Players[serverId];
            if (player.Handle != Game.Player.Handle)
            {
                switch (API.GetVehicleClassFromName((uint)API.GetHashKey(vehicleName)))
                {
                    case 14:
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 427, false, false);
                        break;
                    case 15:
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 422, false, false);
                        break;
                    case 16:
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 423, false, false);
                        break;
                }

                switch (vehicleName)
                {
                    case "RHINO":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 421, false, false);
                        break;
                    case "INSURGENT":
                    case "INSURGENT2":
                    case "INSURGENT3":
                    case "TECHNICAL":
                    case "TECHNICAL3":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 426, false, false);
                        break;
                    case "LIMO2":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 460, false, false);
                        break;
                    case "PHANTOM2":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 528, false, false);
                        break;
                    case "BOXVILLE5":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 529, false, false);
                        break;
                    case "RUINER2":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 530, false, false);
                        break;
                    case "DUNE4":
                    case "DUNE5":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 531, false, false);
                        break;
                    case "ZHABA":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 532, false, false);
                        break;
                    case "VOLTIC2":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 533, false, false);
                        break;
                    case "TECHNICAL2":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 534, false, false);
                        break;
                    case "APC":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 558, false, false);
                        break;
                    case "OPPRESSOR":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 559, false, false);
                        break;
                    case "HALFTRACK":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 560, false, false);
                        break;
                    case "STROMBERG":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 595, false, false);
                        break;
                    case "DELUXO":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 596, false, false);
                        break;
                    case "BRUISER":
                    case "BRUISER2":
                    case "BRUISER3":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 658, false, false);
                        break;
                    case "BRUTUS":
                    case "BRUTUS2":
                    case "BRUTUS3":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 659, false, false);
                        break;
                    case "CERBERUS":
                    case "CERBERUS2":
                    case "CERBERUS3":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 660, false, false);
                        break;
                    case "DEATHBIKE":
                    case "DEATHBIKE2":
                    case "DEATHBIKE3":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 661, false, false);
                        break;
                    case "DOMINATOR4":
                    case "DOMINATOR5":
                    case "DOMINATOR6":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 662, false, false);
                        break;
                    case "IMPALER2":
                    case "IMPALER3":
                    case "IMPALER4":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 663, false, false);
                        break;
                    case "IMPERATOR":
                    case "IMPERATOR2":
                    case "IMPERATOR3":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 664, false, false);
                        break;
                    case "ISSI4":
                    case "ISSI5":
                    case "ISSI6":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 665, false, false);
                        break;
                    case "MONSTER3":
                    case "MONSTER4":
                    case "MONSTER5":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 666, false, false);
                        break;
                    case "SCARAB":
                    case "SCARAB2":
                    case "SCARAB3":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 667, false, false);
                        break;
                    case "SLAMVAN4":
                    case "SLAMVAN5":
                    case "SLAMVAN6":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 668, false, false);
                        break;
                    case "ZR380":
                    case "ZR3802":
                    case "ZR3803":
                        SetBlip(player, API.GetBlipFromEntity(player.Character.Handle), 669, false, false);
                        break;
                }
            }
        }

        // Remove the player blip
        private void RemovePlayerBlip(int serverId)
        {
            Player player = Players[serverId];
            Debug.WriteLine($"Removing blip for {player.Name}");
            //int blipToRemove = API.GetBlipFromEntity(Players[serverId].Character.Handle);
            //
            player.Character.AttachedBlip.Delete();
        }

        private bool IsBlipRotating(int blip)
        {
            bool isRotating = false;
            switch (API.GetBlipSprite(blip))
            {
                case 421:
                case 423:
                case 424:
                case 426:
                case 427:
                case 533:
                case 534:
                case 558:
                case 562:
                case 572:
                case 573:
                case 575:
                case 577:
                case 578:
                case 579:
                case 580:
                case 581:
                case 582:
                case 583:
                case 584:
                case 585:
                case 598:
                case 600:
                case 601:
                case 613:
                case 646:
                    isRotating = true;
                    break;
                default:
                    isRotating = false;
                    break;
            }

            return isRotating;
        }

        public async void Update()
        {
            foreach(Player player in Players)
            {
                if (API.DoesBlipExist(API.GetBlipFromEntity(player.Character.Handle)))
                {
                    if (IsBlipRotating(API.GetBlipFromEntity(player.Character.Handle)))
                    {
                        // Rotate to player's heading
                        API.SetBlipRotation(API.GetBlipFromEntity(player.Character.Handle), (int)Math.Ceiling(API.GetEntityHeading(player.Character.Handle)));
                    }
                }
            }
        }
    }
}
