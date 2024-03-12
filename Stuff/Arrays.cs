// Author: Scottywonderful
// Created: 10th Mar 2024
// Version: 0.4.7.2

#region

#endregion

namespace SWLCallouts.Stuff;

internal class Arrays
{
    internal static readonly string[] PluginLoadText =
    {
        "Hi",
        "",
    };
    
    internal static readonly string[] PluginUnloadText =
    {
        "Bye",
    };

    internal static readonly string[] SFDispatchArrive;
    internal static readonly string[] SFDispatchCode4;
    internal static readonly string[] SEV1DispatchArrive;
    internal static readonly string[] SEV1DispatchCode4;
    internal static readonly string[] SEV2DispatchArrive;
    internal static readonly string[] SEV2DispatchCode4;
    internal static readonly string[] WCDispatchArrive;
    internal static readonly string[] WCDispatchCode4;

    static Arrays()
    {
        // Read the value of ActivateAIBackup from SWLCallouts.ini //
        Log("Getting AIBackup Settings");
        var ini = new InitializationFile("Plugins/LSPDFR/SWLCallouts.ini");
        bool activateAIBackup = ini.ReadBoolean("Settings", "ActivateAIBackup", true);

        // Initialise Dispatch for .ShotsFired. Callout based on activateAIBackup
        SFDispatchArrive = activateAIBackup ?
            new string[]
            {
                "Copy. Requesting you investigate the area.",
                ""
            } :
            new string[]
            {
                "",
                ""
            };
        SFDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "",
                ""
            } :
            new string[]
            {
                "",
                ""
            };

        // Initialise Dispatch Arrive for .StolenEmergencyVehicle1. Callout based on activateAIBackup
        SEV1DispatchArrive = activateAIBackup ?
            new string[]
            {
                "",
                ""
            } :
            new string[]
            {
                "",
                ""
            };
        SEV1DispatchCode4 = activateAIBackup ?
            new string[]
            {
                "",
                ""
            } :
            new string[]
            {
                "",
                ""
            };

        // Initialise Dispatch Arrive for .StolenEmergencyVehicle2. Callout based on activateAIBackup
        SEV2DispatchArrive = activateAIBackup ?
            new string[]
            {
                "",
                ""
            } :
            new string[]
            {
                "",
                ""
            };
        SEV2DispatchCode4 = activateAIBackup ?
            new string[]
            {
                "",
                ""
            } :
            new string[]
            {
                "",
                ""
            };

        // Initialise Dispatch Arrive for .WelfareCheck. Callout based on activateAIBackup
        WCDispatchArrive = activateAIBackup ?
            new string[]
            {
                "We have an ~y~ambulance~w~ on the way to your current location. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "An ~y~ambulance~w~ is enroute to the location. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "An ~y~ambulance~w~ is enroute to your current location, officer. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "~y~Ambulance~w~ has been dispatched to you officer. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "~y~Ambulance~w~ enroute! You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "We are going to call an ~y~ambulance~w~ to your current location, officer. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                // End Call button sentence change //
                "We have an ~y~ambulance~w~ on the way to your current location. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "An ~y~ambulance~w~ is enroute to the location. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "An ~y~ambulance~w~ is enroute to your current location, officer. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "~y~Ambulance~w~ has been dispatched to you officer. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "~y~Ambulance~w~ enroute! You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "We are going to call an ~y~ambulance~w~ to your current location, officer. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
            } :
            new string[]
            {
                "We see you on scene officer, check the area and return to patrol. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "Check the area and return to patrol officer. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "Investigate the scene and report back, officer. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "Please search the area and report your findings. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "Officer, search the area and request for assistance if needed. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "We see you arriving on scene officer, check the area and report your findings. You may end the call by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                // End Call button sentence change //
                "We see you on scene officer, check the area and return to patrol. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "Check the area and return to patrol officer. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "Investigate the scene and report back, officer. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "Please search the area and report your findings. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "Officer, search the area and request for assistance if needed. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",
                "We see you arriving on scene officer, check the area and report your findings. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key.",

            };
        WCDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~b~ ~w~Dispatch we're code 4. Show me ~g~10-8.",
                "",
            } :
            new string[]
            {
                "",
                ""
            };
    }

    /// <summary>
    /// List of every weapon needed
    /// </summary>
    internal static readonly string[] WeaponList = {
		// melee //
        "weapon_dagger",
        "weapon_bat",
        "weapon_bottle",
        "weapon_crowbar",
        "weapon_golfclub",
        "weapon_hammer",
        "weapon_hatchet",
        "weapon_knife",
        "weapon_machete",
        "weapon_switchblade",
        "weapon_nightstick",
        "weapon_wrench",
        "weapon_battleaxe",
        //"weapon_poolcue", //Pubs/clubs only..
        "weapon_stone_hatchet",
        //"weapon_candycane", //Unrealistic

		// pistols //
        "weapon_pistol",
        "weapon_pistol_mk2",
        "weapon_combatpistol",
        "weapon_appistol",
        //"weapon_stungun", //police weapon
        "weapon_pistol50",
        "weapon_snspistol",
        "weapon_snspistol_mk2",
        "weapon_heavypistol",
        //"weapon_vintagepistol", // Some people use this as a radar gun //
        "weapon_marsmanpistol",
        "weapon_revolver",
        "weapon_revolver_mk2",
        "weapon_doubleaction",
        //"weapon_raypistol", //Unrealistic
        "weapon_ceramicpistol",
        "weapon_navyrevolver",
        //"weapon_gadgetpistol", //Unrealistic
        //"weapon_stungun_mp", //police weapon
        "weapon_pistolxm3",

        // submachineguns //
        "weapon_microsmg",
        "weapon_smg",
        "weapon_smg_mk2",
        "weapon_assaultsmg",
        "weapon_combatpdw",
        "weapon_machinepistol",
        "weapon_minismg",
        //"weapon_raycarbine", //Unrealistic
        "weapon_tecpistol",

        // shotguns //
        "weapon_pumpshotgun",
        "weapon_pumpshotgun_mk2",
        "weapon_sawnoffshotgun",
        "weapon_assaultshotgun",
        "weapon_bullpupshotgun",
        "weapon_heavyshotgun",
        "weapon_musket",
        "weapon_dbshotgun",
        "weapon_autoshotgun",
        "weapon_combatshotgun",

        // assault rifles //
        "weapon_assaultrifle",
        "weapon_assaultrifle_mk2",
        "weapon_carbinerifle",
        "weapon_carbinerifle_mk2",
        "weapon_advancedrifle",
        "weapon_specialcarbine",
        "weapon_specialcarbine_mk2",
        "weapon_bullpuprifle",
        "weapon_bullpuprifle_mk2",
        "weapon_compactrifle",
        "weapon_militaryrifle",
        "weapon_heavyrifle",
        "weapon_tacticalrifle",

        // light machine guns //
        "weapon_mg",
        "weapon_combatmg",
        "weapon_combatmg_mk2",
        "weapon_gusenberg",

        // sniper rifles //
        "weapon_sniperrifle",
        "weapon_heavysniper",
        "weapon_heavysniper_mk2",
        "weapon_marksmanrifle",
        "weapon_marksmanrifle_mk2",
        "weapon_precisionrifle",

        // heavy weapons //
        //"weapon_rpg",
        //"weapon_grenadelauncher", //Unrealistic
        //"weapon_grenadelauncher_smoke", //Unrealistic
        //"weapon_minigun",
        //"weapon_firework", //Unrealistic
        //"weapon_railgun", //Unrealistic
        //"weapon_hominglauncher", //Unrealistic
        //"weapon_compactlauncher", //Unrealistic
        //"weapon_rayminigun", //Unrealistic
        //"weapon_emplauncher", //Unrealistic
        //"weapon_railgunxm3", //Unrealistic
    };

    internal static readonly string[] MeleeWeapons =
        {
        "weapon_dagger",
        "weapon_bat",
        "weapon_bottle",
        "weapon_crowbar",
        "weapon_golfclub",
        "weapon_hammer",
        "weapon_hatchet",
        "weapon_knife",
        "weapon_machete",
        "weapon_switchblade",
        "weapon_nightstick",
        "weapon_wrench",
        "weapon_battleaxe",
        //"weapon_poolcue", //Pubs/clubs only..
        "weapon_stone_hatchet",
        //"weapon_candycane", //Unrealistic
    };

    internal static readonly string[] PistolWeapons =
        {
        "weapon_pistol",
        "weapon_pistol_mk2",
        "weapon_combatpistol",
        "weapon_appistol",
        //"weapon_stungun", //police weapon
        "weapon_pistol50",
        "weapon_snspistol",
        "weapon_snspistol_mk2",
        "weapon_heavypistol",
        //"weapon_vintagepistol", // Some people use this as a radar gun //
        "weapon_marsmanpistol",
        "weapon_revolver",
        "weapon_revolver_mk2",
        "weapon_doubleaction",
        //"weapon_raypistol", //Unrealistic
        "weapon_ceramicpistol",
        "weapon_navyrevolver",
        //"weapon_gadgetpistol", //Unrealistic
        //"weapon_stungun_mp", //police weapon
        "weapon_pistolxm3",
    };

    internal static readonly string[] LightWeapons =
        {
        // submachineguns //
        "weapon_microsmg",
        "weapon_smg",
        "weapon_smg_mk2",
        "weapon_assaultsmg",
        "weapon_combatpdw",
        "weapon_machinepistol",
        "weapon_minismg",
        //"weapon_raycarbine", //Unrealistic
        "weapon_tecpistol",
        // shotguns //
        "weapon_pumpshotgun",
        "weapon_pumpshotgun_mk2",
        "weapon_sawnoffshotgun",
        "weapon_musket",
        "weapon_dbshotgun",
        // assault rifles //
        "weapon_compactrifle",
        "weapon_gusenberg",
    };

    internal static readonly string[] HeavyWeapons =
    {
        // shotguns //
        "weapon_assaultshotgun",
        "weapon_bullpupshotgun",
        "weapon_heavyshotgun",
        "weapon_autoshotgun",
        "weapon_combatshotgun",
        // assault rifles //
        "weapon_assaultrifle",
        "weapon_assaultrifle_mk2",
        "weapon_carbinerifle",
        "weapon_carbinerifle_mk2",
        "weapon_advancedrifle",
        "weapon_specialcarbine",
        "weapon_specialcarbine_mk2",
        "weapon_bullpuprifle",
        "weapon_bullpuprifle_mk2",
        "weapon_militaryrifle",
        "weapon_heavyrifle",
        "weapon_tacticalrifle",
        // light machine guns //
        "weapon_mg",
        "weapon_combatmg",
        "weapon_combatmg_mk2",
        "weapon_gusenberg",
        // sniper rifles //
        "weapon_sniperrifle",
        "weapon_heavysniper",
        "weapon_heavysniper_mk2",
        "weapon_marksmanrifle",
        "weapon_marksmanrifle_mk2",
        "weapon_precisionrifle",
        // heavy weapons //
        "weapon_rpg",
        //"weapon_grenadelauncher", //Unrealistic
        //"weapon_grenadelauncher_smoke", //Unrealistic
        "weapon_minigun",
        //"weapon_firework", //Unrealistic
        //"weapon_railgun", //Unrealistic
        //"weapon_hominglauncher", //Unrealistic
        //"weapon_compactlauncher", //Unrealistic
        //"weapon_rayminigun", //Unrealistic
        //"weapon_emplauncher", //Unrealistic
        //"weapon_railgunxm3", //Unrealistic
    };
}
