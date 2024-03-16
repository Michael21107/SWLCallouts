// Author: Scottywonderful
// Created: 10th Mar 2024
// Version: 0.4.8.5

#region

#endregion

namespace SWLCallouts.Stuff;

internal class Arrays
{
    internal static readonly string[] PluginLoadText =
    {
        $"~w~Seems like you have the ~g~latest version ~b~{Settings.PluginVersion}~w~!<br>Thanks for downloading partner, stay safe onduty.",
        $"~g~Latest version ~b~{Settings.PluginVersion}~w~, aye!<br>Stay safe out there officer.",
        "~b~Are you sure you wanna go onduty?",
        "~w~Damn I think a ~b~bluey ~w~is a thing, ~r~right?",
        "~p~*In robot voice*<br>~w~Beep Boop, You are on duty!",
        "~w~Dispatch, show this awesome officer on duty!<br>~g~Thank you ~w~and stay safe!"
    };

    internal static readonly string[] PluginLoadTestingSubtitle =
{
        "~p~by SWL Creations",
        "~p~Ok, testing it aye!",
        "~p~Testing mode activated!",
        "~p~Bruh! What you doing?",
        "~p~Hello tester!",
        "~p~OH! I see how it is!"
    };

    internal static readonly string[] PluginLoadTestingText =
    {
        "~g~Thank you ~b~for helping ~p~me out ~o~by testing ~w~this wonderful script!",
        "~b~OMG! ~g~Thank you ~w~for helping me out with testing!",
        "~y~You know! ~w~you're an absolute legend for testing this for me.",
        "~r~Red ~w~is for a Rose, ~b~Blue ~w~is for you, ~p~Purple ~w~is for me. ~y~Look at you testing, oh my g!",
        "~w~Is this thing on? ~y~Hello?! ~r~HEELLLLOOOOO!!",
        "~w~Guess we coded this right for you to use."
    };

    internal static readonly string[] PluginUnloadText =
    {
        "~r~Oh shit! ~y~Was that a bomb?",
        "~r~WOOW! ~w~What happened?",
        "~p~I guess you crashed..",
        "~g~Green~w~, ~y~Yellow~w~, ~r~Red~w~. Did you know that red means stop?",
        "~w~I think that was a ~r~red ~y~lig~g~ht ~w~back there.",
        "~w~Welp, that didn't last long.. Not long enough."
    };

    internal static readonly string[] CalloutNoAnswer =
    {
        "AI_ADAM5_TAKING_CALL",
        "AI_OCEAN1_TAKING_CALL_01",
        "AI_OCEAN1_TAKING_CALL_02",
        "AI_QUEEN2_TAKING_CALL",
        "AI_UNIT_TAKING_CALL_01",
        "AI_UNIT_TAKING_CALL_02",
        "AI_UNIT_TAKING_CALL_03",
        "UNIT_RESPONDING_DISPATCH_01",
        "UNIT_RESPONDING_DISPATCH_02",
        "UNIT_RESPONDING_DISPATCH_03",
        "UNIT_RESPONDING_DISPATCH_04",
    };

    internal static readonly string[] CalloutAIBackup =
    {
        "REQUESTING_BACKUP_01",
        "REQUESTING_BACKUP_02",
        "REQUESTING_BACKUP_03",
        "REQUESTING_BACKUP_01",
        "REQUESTING_BACKUP_01",
        "REQUESTING_BACKUP_01",
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
        Normal("Getting AIBackup Settings");
        var ini = new InitializationFile("Plugins/LSPDFR/SWLCallouts.ini");
        bool activateAIBackup = ini.ReadBoolean("Settings", "ActivateAIBackup", true);

        // Initialise Dispatch for .ShotsFired. Callout based on activateAIBackup
        SFDispatchArrive = activateAIBackup ?
            new string[]
            {
                "Copy. Requesting you investigate the area.",
                "",
                "",
                "",
                "",
                ""
            } :
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            };
        SFDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            } :
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            };

        // Initialise Dispatch Arrive for .StolenEmergencyVehicle1. Callout based on activateAIBackup
        SEV1DispatchArrive = activateAIBackup ?
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            } :
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            };
        SEV1DispatchCode4 = activateAIBackup ?
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            } :
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            };

        // Initialise Dispatch Arrive for .StolenEmergencyVehicle2. Callout based on activateAIBackup
        SEV2DispatchArrive = activateAIBackup ?
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            } :
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            };
        SEV2DispatchCode4 = activateAIBackup ?
            new string[]
            {
                "",
                "",
                "",
                "",
                "",
                ""
            } :
            new string[]
            {
                "",
                "",
                "",
                "",
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
                "We are going to call an ~y~ambulance~w~ to your current location, officer. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key."
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
                "We see you arriving on scene officer, check the area and report your findings. You may return to patrol by pressing the ~y~" + Settings.EndCall + "~w~ key."
            };
        WCDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~b~Dispatch: ~w~All units, be advised, scene is code 4. No additional units required!",
                "~b~Dispatch: ~w~We are showing all units back on patrol.",
                "~b~Dispatch: ~w~Officer, you may all return to patrol.",
                "~b~Dispatch: ~w~Thank you officer, all units back available.",
                "~b~Dispatch: ~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~b~Dispatch: ~w~10-4, Showing you all available."
            } :
            new string[]
            {
                "~b~Dispatch: ~w~Showing you code 4 on scene",
                "~b~Dispatch: ~w~You may stay on scene officer though you should return to patrol.",
                "~b~Dispatch: ~w~Copy that, showing you back on patrol.",
                "~b~Dispatch: ~w~Scene is clear, awaiting your next call.",
                "~b~Dispatch: ~w~Officer, you are required to return to patrol.",
                "~b~Dispatch: ~w~Code 4, All units back on patrol."
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
        "weapon_poolcue",
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
        "weapon_vintagepistol", // Some people use this as a radar gun //
        "weapon_marksmanpistol",
        "weapon_revolver",
        "weapon_revolver_mk2",
        "weapon_doubleaction",
        //"weapon_raypistol", //Unrealistic
        "weapon_ceramicpistol",
        "weapon_navyrevolver",
        "weapon_gadgetpistol",
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
        //"weapon_tecpistol",

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
        "weapon_poolcue",
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
        "weapon_vintagepistol", // Some people use this as a radar gun //
        "weapon_marsmanpistol",
        "weapon_revolver",
        "weapon_revolver_mk2",
        "weapon_doubleaction",
        //"weapon_raypistol", //Unrealistic
        "weapon_ceramicpistol",
        "weapon_navyrevolver",
        "weapon_gadgetpistol",
        //"weapon_stungun_mp", //police weapon
        "weapon_pistolxm3",
    };

    internal static readonly string[] PisleeWeapons =
        {
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
        "weapon_poolcue",
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
        "weapon_vintagepistol", // Some people use this as a radar gun //
        "weapon_marsmanpistol",
        "weapon_revolver",
        "weapon_revolver_mk2",
        "weapon_doubleaction",
        //"weapon_raypistol", //Unrealistic
        "weapon_ceramicpistol",
        "weapon_navyrevolver",
        "weapon_gadgetpistol",
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
