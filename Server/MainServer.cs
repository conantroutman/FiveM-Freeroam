using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Server
{
    public class MainServer : BaseScript
    {
        List<int> pistols = new List<int>
        {
            GetHashKey("weapon_pistol"),
            GetHashKey("weapon_pistol_mk2"),
            GetHashKey("weapon_combatpistol"),
            GetHashKey("weapon_appistol"),
            GetHashKey("weapon_pistol50"),
            GetHashKey("weapon_snspistol"),
            GetHashKey("weapon_snspistol_mk2"),
            GetHashKey("weapon_heavypistol"),
            GetHashKey("weapon_vintagepistol"),
            GetHashKey("weapon_marksmanpistol"),
            GetHashKey("weapon_revolver"),
            GetHashKey("weapon_revolver_mk2"),
            GetHashKey("weapon_doubleaction"),
            GetHashKey("weapon_ceramicpistol"),
            GetHashKey("weapon_navyrevolver")
        };

        List<int> SMGs = new List<int>
        {
            GetHashKey("weapon_microsmg"),
            GetHashKey("weapon_smg"),
            GetHashKey("weapon_smg_mk2"),
            GetHashKey("weapon_assaultsmg"),
            GetHashKey("weapon_combatpdw"),
            GetHashKey("weapon_machinepistol"),
            GetHashKey("weapon_minismg"),
            GetHashKey("weapon_raycarbine"),
        };

        List<int> shotguns = new List<int>
        {
            GetHashKey("weapon_pumpshotgun"),
            GetHashKey("weapon_pumpshotgun_mk2"),
            GetHashKey("weapon_sawnoffshotgun"),
            GetHashKey("weapon_assaultshotgun"),
            GetHashKey("weapon_bullpupshotgun"),
            GetHashKey("weapon_musket"),
            GetHashKey("weapon_heavyshotgun"),
            GetHashKey("weapon_dbshotgun"),
            GetHashKey("weapon_autoshotgun"),
            GetHashKey("weapon_combatshotgun")
        };

        List<int> assaultRifles = new List<int>
        {
            GetHashKey("weapon_assaultrifle"),
            GetHashKey("weapon_assaultrifle_mk2"),
            GetHashKey("weapon_carbinerifle"),
            GetHashKey("weapon_carbinerifle_mk2"),
            GetHashKey("weapon_advancedrifle"),
            GetHashKey("weapon_specialcarbine"),
            GetHashKey("weapon_specialcarbine_mk2"),
            GetHashKey("weapon_bullpuprifle"),
            GetHashKey("weapon_bullpuprifle_mk2"),
            GetHashKey("weapon_compactrifle"),
            GetHashKey("weapon_militaryrifle")
        };

        List<int> LMGs = new List<int>
        {
            GetHashKey("weapon_mg"),
            GetHashKey("weapon_combatmg"),
            GetHashKey("weapon_combatmg_mk2"),
            GetHashKey("weapon_gusenberg"),
        };

        List<int> snipers = new List<int>
        {
            GetHashKey("weapon_sniperrifle"),
            GetHashKey("weapon_heavysniper"),
            GetHashKey("weapon_heavysniper_mk2"),
            GetHashKey("weapon_marksmanrifle"),
            GetHashKey("weapon_marksmanrifle_mk2"),
        };

        List<int> meleeSharp = new List<int>
        {
            GetHashKey("weapon_dagger"),
            GetHashKey("weapon_bottle"),
            GetHashKey("weapon_knife"),
            GetHashKey("weapon_hatchet"),
            GetHashKey("weapon_knife"),
            GetHashKey("weapon_machete"),
            GetHashKey("weapon_switchblade"),
            GetHashKey("weapon_battleaxe"),
            GetHashKey("weapon_stone_hatchet"),
        };

        List<int> meleeBlunt = new List<int>
        {
            GetHashKey("weapon_bat"),
            GetHashKey("weapon_crowbar"),
            GetHashKey("weapon_flashlight"),
            GetHashKey("weapon_golfclub"),
            GetHashKey("weapon_hammer"),
            GetHashKey("weapon_nightstick"),
            GetHashKey("weapon_wrench"),
            GetHashKey("weapon_poolcue"),
            GetHashKey("weapon_unarmed"),
            GetHashKey("weapon_knuckle"),
        };

        public MainServer()
        {
            Debug.WriteLine("Started the server.");

            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);

            EventHandlers["baseevents:onPlayerDied"] += new Action<Player>(OnPlayerDied);
            EventHandlers["baseevents:onPlayerKilled"] += new Action<Player, int, dynamic>(OnPlayerKilled);

            EventHandlers["playerRespawned"] += new Action<Player>(OnPlayerRespawn);

            EventHandlers["baseevents:enteredVehicle"] += new Action<Player, int, int, string>(OnEnteredVehicle);
            EventHandlers["baseevents:leftVehicle"] += new Action<Player>(OnLeftVehicle);

            EventHandlers["basejumping:playerLanded"] += new Action<Player, int, int>(OnBaseJumpPlayerLanded);

        }

        private void OnBaseJumpPlayerLanded([FromSource] Player player, int distance, int glideTime)
        {
            Debug.WriteLine($"{player.Name} completed a base jump.");
            TriggerClientEvent("basejumping:onLanding", player.Handle, distance, glideTime);
        }

        private async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            // mandatory wait!
            await Delay(0);

            var licenseIdentifier = player.Identifiers["license"];

            Debug.WriteLine($"A player with the name {playerName} (Identifier: [{licenseIdentifier}], ID: {player.Handle}) is connecting to the server.");

            deferrals.update($"Hello {playerName}, your license [{licenseIdentifier}] is being checked");

            // Checking ban list
            // - assuming you have a function called IsBanned of type Task<bool>
            // - normally you'd do a database query here, which might take some time
            /*if (await IsBanned(licenseIdentifier))
            {
                deferrals.done($"You have been kicked (Reason: [Banned])! Please contact the server administration (Identifier: [{licenseIdentifier}]).");
            }*/

            deferrals.done();

            TriggerClientEvent("playerJoined", $"~HUD_COLOUR_NET_PLAYER{player.Handle}~<C>{player.Name}</C>~w~ joined.");
        }

        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            Debug.WriteLine($"Player {player.Name} dropped (Reason: {reason}).");

            TriggerClientEvent("playerLeft", player.Handle, $"~HUD_COLOUR_NET_PLAYER{player.Handle}~<C>{player.Name}</C>~w~ left.");
        }

        private void OnPlayerDied([FromSource] Player victim)
        {
            Debug.WriteLine($"{victim.Name} died.");
            TriggerClientEvent("playerDied", victim.Handle);
        }

        private void OnPlayerKilled([FromSource] Player victim, int killerId, dynamic deathData)
        {
            // deathData contents:
            //   killertype
            //   weaponhash = the murder weapon
            //   killerinveh = was the killer in a vehicle
            //   killervehseat
            //   killervehname = name of the killer's vehicle
            //   killerpos = killer's coordinates
            Debug.WriteLine($"{GetKillerWeaponType((int)deathData.weaponhash)}");
            TriggerClientEvent("playerKilled", victim.Handle, deathData.weaponhash, GetKillTerm(GetKillerWeaponType((int)deathData.weaponhash)));
        }

        private void OnPlayerRespawn([FromSource] Player player)
        {
            Debug.WriteLine($"{player.Name} respawned.");
            TriggerClientEvent("playerRespawned", player.Handle);
        }

        private void OnEnteredVehicle([FromSource] Player player, int vehicle, int seat, string name)
        {
            Debug.WriteLine($"{player.Name} entered a {name}.");
            TriggerClientEvent("playerEnteredVehicle", player.Handle, name);
        }

        private void OnLeftVehicle([FromSource] Player player)
        {
            TriggerClientEvent("playerLeftVehicle", player.Handle);
        }

        private int GetKillerWeaponType(int weaponHash)
        {
            if (meleeBlunt.Contains(weaponHash))
            {
                return 1;
            }
            else if (meleeSharp.Contains(weaponHash))
            {
                return 2;
            }
            else if (pistols.Contains(weaponHash))
            {
                return 3;
            }
            else if (SMGs.Contains(weaponHash))
            {
                return 4;
            }
            else if (shotguns.Contains(weaponHash))
            {
                return 5;
            }
            else if (assaultRifles.Contains(weaponHash))
            {
                return 6;
            }
            else if (LMGs.Contains(weaponHash))
            {
                return 7;
            }
            else if (snipers.Contains(weaponHash))
            {
                return 8;
            }
            else
            {
                return 0;
            }
        }

        private string GetKillTerm(int weaponType)
        {
            List<string> meleeBluntTerms = new List<string>
            {
                "battered",
                "beat down",
                "bludgeoned",
                "brained",
                "broke",
                "cracked",
                "hammered",
                "hemmored",
                "pummeled",
                "smashed",
            };

            List<string> meleeSharpTerms = new List<string>
            {
                "butchered",
                "cut up",
                "diced",
                "eviscerated",
                "filleted",
                "slashed",
                "spiked",
                "stabbed",
                "stuck it to",
                "striped",
            };

            List<string> pistolTerms = new List<string>
            {
                "2nd amendmented",
                "capped",
                "clipped",
                "clocked",
                "deaded",
                "pistoled",
                "popped",
                "shot",
                "smoked",
                "whacked"
            };

            List<string> smgTerms = new List<string>
            {
                "cancelled",
                "crossed out",
                "cut down",
                "massacred",
                "peppered",
                "plugged",
                "riddled",
                "SMGed",
                "sprayed",
                "ventilated",
            };

            List<string> shotgunTerms = new List<string>
            {
                "12 bored",
                "blew away",
                "double barreled",
                "farmed",
                "opened up",
                "perforated",
                "pumped",
                "put a load in",
                "shelled",
                "shottied",
            };

            List<string> assaultRifleTerms = new List<string>
            {
                "3rd worlded",
                "annihilated",
                "armied",
                "decommissioned",
                "drilled",
                "ended",
                "killed",
                "machined",
                "punctuated",
                "terminated",
            };

            List<string> lmgTerms = new List<string>
            {
                "machine gunned",
                "sprayed"
            };

            string killTerm;
            var random = new Random();

            switch (weaponType)
            {
                case 1:
                    killTerm = meleeBluntTerms[random.Next(meleeBluntTerms.Count)];
                    break;
                case 2:
                    killTerm = meleeSharpTerms[random.Next(meleeSharpTerms.Count)];
                    break;
                case 3:
                    killTerm = pistolTerms[random.Next(pistolTerms.Count)];
                    break;
                case 4:
                    killTerm = smgTerms[random.Next(smgTerms.Count)];
                    break;
                case 5:
                    killTerm = shotgunTerms[random.Next(shotgunTerms.Count)];
                    break;
                case 6:
                    killTerm = assaultRifleTerms[random.Next(assaultRifleTerms.Count)];
                    break;
                case 7:
                    killTerm = pistolTerms[random.Next(pistolTerms.Count)];
                    break;
                case 8:
                    killTerm = pistolTerms[random.Next(pistolTerms.Count)];
                    break;
                case 9:
                    killTerm = pistolTerms[random.Next(pistolTerms.Count)];
                    break;
                default:
                    killTerm = "killed";
                    break;
            }

            return killTerm;
        }
    }
}
