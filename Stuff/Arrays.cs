// Author: Scottywonderful
// Created: 10th Mar 2024
// Version: 0.5.0.1

#region

#endregion

using System.Windows.Forms;

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
        "~g~Green~w~, ~y~Yellow~w~, ~r~Red~w~. Did you know that ~r~red~w~ means stop?",
        "~w~I think that was a ~r~red ~y~lig~g~ht ~w~back there.",
        "~w~Welp, that didn't last long.. Not long enough."
    };

    internal static readonly string[] AIOfficerEnroute =
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

    internal static readonly string[] PWAKSuspectSpeech =
    {
        "~r~Suspect: ~w~I don't want to live anymore! So Shoot me already!",
        "~r~Suspect: ~w~Go away! You are not going to take me alive!",
        "~r~Suspect: ~w~I'm not going back to that padded cell!",
        "~r~Suspect: ~w~You are going to die pig!",
        "~r~Suspect: ~w~You came to the wrong place arsehole!",
        "~r~Suspect: ~w~What do you want? Wanna fight? Let's go!",
        "~r~Suspect: ~w~Cops thinking they can always help!",
        "~r~Suspect: ~w~What you gonna do? Shoot me?",
        "~r~Suspect: ~w~Shoot me! Get it over with!",
        "~r~Suspect: ~w~Come on then, bring it! Shoot me!"
    };

    internal static readonly string[] COTMDispatchArrive;
    internal static readonly string[] COTMDispatchCode4;
    internal static readonly string[] HSCDispatchArrive;
    internal static readonly string[] HSCDispatchCode4;
    internal static readonly string[] MIDispatchArrive;
    internal static readonly string[] MIDispatchCode4;
    internal static readonly string[] PWAKDispatchArrive;
    internal static readonly string[] PWAKDispatchCode4;
    internal static readonly string[] SFDispatchArrive;
    internal static readonly string[] SFDispatchCode4;
    internal static readonly string[] SEV1DispatchArrive;
    internal static readonly string[] SEV1DispatchCode4;
    internal static readonly string[] SEV2DispatchArrive;
    internal static readonly string[] SEV2DispatchCode4;
    internal static readonly string[] WCDispatchArriveS1;
    internal static readonly string[] WCDispatchArriveS2;
    internal static readonly string[] WCDispatchArriveS3;
    internal static readonly string[] WCDispatchCode4;

    static Arrays()
    {
        // Read the value of ActivateAIBackup from SWLCallouts.ini //
        Normal("Getting AIBackup Settings");
        var ini = new InitializationFile("Plugins/LSPDFR/SWLCallouts.ini");
        bool activateAIBackup = ini.ReadBoolean("Settings", "ActivateAIBackup", true);

        // Initialise Dispatch for .ShotsFired. Callout based on activateAIBackup
        COTMDispatchArrive = activateAIBackup ?
            new string[]
            {
                "AIMessage1",
                "AIMessage2",
                "AIMessage3",
                "AIMessage4",
                "AIMessage5",
                "AIMessage6"
            } :
            new string[]
            {
                "Message1",
                "Message2",
                "Message3",
                "Message4",
                "Message5",
                "Message6"
            };
        COTMDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~w~All units, be advised, scene is code 4. No additional units required!",
                "~w~We are showing all units back on patrol.",
                "~w~Officer, you may all return to patrol.",
                "~w~Thank you officer, all units back available.",
                "~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~w~10-4, Showing you all available."
            } :
            new string[]
            {
                ": ~w~Showing you code 4 on scene",
                "~w~You may stay on scene officer though you should return to patrol.",
                "~w~Copy that, showing you back on patrol.",
                "~w~Scene is clear, awaiting your next call.",
                "~w~Officer, you are required to return to patrol.",
                "~w~Code 4, All units back on patrol."
            };

        // Initialise Dispatch for .ShotsFired. Callout based on activateAIBackup
        HSCDispatchArrive = activateAIBackup ?
            new string[]
            {
                "AIMessage1",
                "AIMessage2",
                "AIMessage3",
                "AIMessage4",
                "AIMessage5",
                "AIMessage6"
            } :
            new string[]
            {
                "Message1",
                "Message2",
                "Message3",
                "Message4",
                "Message5",
                "Message6"
            };
        HSCDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~w~All units, be advised, scene is code 4. No additional units required!",
                "~w~We are showing all units back on patrol.",
                "~w~Officer, you may all return to patrol.",
                "~w~Thank you officer, all units back available.",
                "~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~w~10-4, Showing you all available."
            } :
            new string[]
            {
                "~w~Showing you code 4 on scene",
                "~w~You may stay on scene officer though you should return to patrol.",
                "~w~Copy that, showing you back on patrol.",
                "~w~Scene is clear, awaiting your next call.",
                "~w~Officer, you are required to return to patrol.",
                "~w~Code 4, All units back on patrol."
            };

        // Initialise Dispatch for .ShotsFired. Callout based on activateAIBackup
        MIDispatchArrive = activateAIBackup ?
            new string[]
            {
                "Showing you on scene, officers are on scene.",
                "If suspect is on scene, please advise, additional units in the area.",
                "AIMessage3",
                "AIMessage4",
                "AIMessage5",
                "AIMessage6"
            } :
            new string[]
            {
                "Message1",
                "Message2",
                "Message3",
                "Message4",
                "Message5",
                "Message6"
            };
        MIDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~w~All units, be advised, scene is code 4. No additional units required!",
                "~w~We are showing all units back on patrol.",
                "~w~Officer, you may all return to patrol.",
                "~w~Thank you officer, all units back available.",
                "~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~w~10-4, Showing you all available."
            } :
            new string[]
            {
                "~w~Showing you code 4 on scene",
                "~w~You may stay on scene officer though you should return to patrol.",
                "~w~Copy that, showing you back on patrol.",
                "~w~Scene is clear, awaiting your next call.",
                "~w~Officer, you are required to return to patrol.",
                "~w~Code 4, All units back on patrol."
            };

        // Initialise Dispatch for .ShotsFired. Callout based on activateAIBackup
        PWAKDispatchArrive = activateAIBackup ?
            new string[]
            {
                "Be aware, person is armed with a knife, proceed with caution.",
                "Showing you on scene, we have an additional unit enroute.",
                "Please be aware that the suspect was last scene welding a knife.",
                "Reminder officer, you are attending a knife call.",
                "Investigate and arrest the suspect officer!",
                "Units, caller has advised that the suspect has threatened someone earlier. Proceed with caution."
            } :
            new string[]
            {
                "Be aware, person is armed with a knife, proceed with caution.",
                "Showing you on scene, officer.",
                "Please be aware that the suspect was last scene welding a knife.",
                "Reminder officer, you are attending a knife call.",
                "Investigate and arrest the suspect officer!",
                "Units, caller has advised that the suspect has threatened someone earlier. Proceed with caution."
            };
        PWAKDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~w~All units, be advised, scene is code 4. No additional units required!",
                "~w~We are showing all units back on patrol.",
                "~w~Officer, you may all return to patrol.",
                "~w~Thank you officer, all units back available.",
                "~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~w~10-4, Showing you all available."
            } :
            new string[]
            {
                "~w~Showing you code 4 on scene",
                "~w~You may stay on scene officer though you should return to patrol.",
                "~w~Copy that, showing you back on patrol.",
                "~w~Scene is clear, awaiting your next call.",
                "~w~Officer, you are required to return to patrol.",
                "~w~Code 4, All units back on patrol."
            };

        // Initialise Dispatch for .ShotsFired. Callout based on activateAIBackup
        SFDispatchArrive = activateAIBackup ?
            new string[]
            {
                "Copy. Requesting you investigate the area.",
                "Backup is enroute! Investigate and if required take down any suspects.",
                "Lethal force authorised officer, suspect seems to be armed.",
                "Showing you on scene officer, backup shouldn't be too far behind you.",
                "We are seeing you arrive on scene officer, proceed with caution.",
                "Additional units enroute. Proceed with caution, lethal force allowed."
            } :
            new string[]
            {
                "Requesting you investigate the area.",
                "Investigate and if required take down any suspects.",
                "Lethal force authorised officer, suspect seems to be armed.",
                "Showing you on scene, officer!",
                "We are seeing you arrive on scene officer, proceed with caution.",
                "Proceed with caution, lethal force allowed."
            };
        SFDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~w~All units, be advised, scene is code 4. No additional units required!",
                "~w~We are showing all units back on patrol.",
                "~w~Officer, you may all return to patrol.",
                "~w~Thank you officer, all units back available.",
                "~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~w~10-4, Showing you all available."
            } :
            new string[]
            {
                "~w~Showing you code 4 on scene",
                "~w~You may stay on scene officer though you should return to patrol.",
                "~w~Copy that, showing you back on patrol.",
                "~w~Scene is clear, awaiting your next call.",
                "~w~Officer, you are required to return to patrol.",
                "~w~Code 4, All units back on patrol."
            };

        // Initialise Dispatch Arrive for .StolenEmergencyVehicle1. Callout based on activateAIBackup
        SEV1DispatchArrive = activateAIBackup ?
            new string[]
            {
                "All units on scene, primary unit is now on scene!",
                "Stop the pursuit without casualties! You are now the primary unit!",
                "Catch up to the pursuit and apprehend the suspect.",
                "Other units are in pursuit, be aware that you have now become primary.",
                "Unit now attaching to call is the primary chase unit.",
                "All units, be advised. Additional support has arrived!"
            } :
            new string[]
            {
                "Units be advised that primary chase offficer is now involved.",
                "All units, be advised, suspect may be armed and dangerous.",
                "Catch up to the pursuit and apprehend the suspect.",
                "We have one unit chasing the vehicle, your job is to apprehend the suspect.",
                "Stop the pursuit without casualties! You are now in charge.",
                "A unit is already in pursuit, you are to assist in the apprehension of the suspect."
            };
        SEV1DispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~w~All units, be advised, scene is code 4. No additional units required!",
                "~w~We are showing all units back on patrol.",
                "~w~Officer, you may all return to patrol.",
                "~w~Thank you officer, all units back available.",
                "~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~w~10-4, Showing you all available."
            } :
            new string[]
            {
                "~w~Showing you code 4 on scene",
                "~w~You may stay on scene officer though you should return to patrol.",
                "~w~Copy that, showing you back on patrol.",
                "~w~Scene is clear, awaiting your next call.",
                "~w~Officer, you are required to return to patrol.",
                "~w~Code 4, All units back on patrol."
            };

        // Initialise Dispatch Arrive for .StolenEmergencyVehicle2. Callout based on activateAIBackup
        SEV2DispatchArrive = activateAIBackup ?
            new string[]
            {
                "All units on scene, primary unit is now on scene!",
                "Stop the pursuit without casualties! You are now the primary unit!",
                "Catch up to the pursuit and apprehend the suspect.",
                "Other units are in pursuit, be aware that you have now become primary.",
                "Unit now attaching to call is the primary chase unit.",
                "All units, be advised. Additional support has arrived!"
            } :
            new string[]
            {
                "Units be advised that primary chase offficer is now involved.",
                "All units, be advised, suspect may be armed and dangerous.",
                "Catch up to the pursuit and apprehend the suspect.",
                "We have one unit chasing the vehicle, your job is to apprehend the suspect.",
                "Stop the pursuit without casualties! You are now in charge.",
                "A unit is already in pursuit, you are to assist in the apprehension of the suspect."
            };
        SEV2DispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~w~All units, be advised, scene is code 4. No additional units required!",
                "~w~We are showing all units back on patrol.",
                "~w~Officer, you may all return to patrol.",
                "~w~Thank you officer, all units back available.",
                "~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~w~10-4, Showing you all available."
            } :
            new string[]
            {
                "~w~Showing you code 4 on scene",
                "~w~You may stay on scene officer though you should return to patrol.",
                "~w~Copy that, showing you back on patrol.",
                "~w~Scene is clear, awaiting your next call.",
                "~w~Officer, you are required to return to patrol.",
                "~w~Code 4, All units back on patrol."
            };

        // Initialise Dispatch Arrive for .WelfareCheck. Callout based on activateAIBackup
        WCDispatchArriveS1 = activateAIBackup ?
            new string[]
            {
                "We have an ~y~ambulance~w~ on the way to your current location.",
                "An ~y~ambulance~w~ is enroute to the location.",
                "An ~y~ambulance~w~ is enroute to your current location, officer.",
                "~y~Ambulance~w~ has been dispatched to you officer.",
                "~y~Ambulance~w~ enroute!",
                "We are going to call an ~y~ambulance~w~ to your current location, officer.",
                // End Call button sentence change //
                "We have an ~y~ambulance~w~ on the way to your current location.",
                "An ~y~ambulance~w~ is enroute to the location.",
                "An ~y~ambulance~w~ is enroute to your current location, officer.",
                "~y~Ambulance~w~ has been dispatched to you officer.",
                "~y~Ambulance~w~ enroute!",
                "We are going to call an ~y~ambulance~w~ to your current location, officer."
            } :
            new string[]
            {
                "We see you on scene officer, check the area and return to patrol.",
                "Check the area and return to patrol officer.",
                "Investigate the scene and report back, officer.",
                "Please search the area and report your findings.",
                "Officer, search the area and request for assistance if needed.",
                "We see you arriving on scene officer, check the area and report your findings.",
                // End Call button sentence change //
                "We see you on scene officer, check the area and return to patrol.",
                "Check the area and return to patrol officer.",
                "Investigate the scene and report back, officer.",
                "Please search the area and report your findings.",
                "Officer, search the area and request for assistance if needed.",
                "We see you arriving on scene officer, check the area and report your findings."
            };
        WCDispatchArriveS2 = activateAIBackup ?
            new string[]
            {
                "Investigate the area. If you don't find anyone, you may ~g~End~w~ the call and return to patrol.",
                "Search the area, if you are unable to find anyone, you may return to patrol. ",
                "We received a hangup call, look around the area. Report your findings and then return to patrol.",
                "Seeing you are on scene officer, investigate, report and return to patrol. ",
                "Possible fake call, check in and then return to patrol.",
                "Officer, proceed with caution, caller hung up and doesn't seem to be answering. Return to patrol if all clear. ",
                "Officer, be on the look out. Caller failed to complete ID check.",
                "We are showing you are on scene, be aware that caller couldn't be reached.",
                "Check the area, if you don't find anything or anyone, you may return to patrol.",
                "Preparing backup unit, report if they are required.",
                "Officer, be aware that caller has been unable to be reached for some time, search the area and report.",
                "Showing you on scene investigating.",
            } :
            new string[]
            {
                "Investigate the area. If you don't find anyone, you may ~g~End~w~ the call and return to patrol.",
                "Search the area, if you are unable to find anyone, you may return to patrol. ",
                "We received a hangup call, look around the area. Report your findings and then return to patrol.",
                "Seeing you are on scene officer, investigate, report and return to patrol. ",
                "Possible fake call, check in and then return to patrol.",
                "Officer, proceed with caution, caller hung up and doesn't seem to be answering. Return to patrol if all clear. ",
                "Officer, be on the look out. Caller failed to complete ID check.",
                "We are showing you are on scene, be aware that caller couldn't be reached.",
                "Check the area, if you don't find anything or anyone, you may return to patrol.",
                "Preparing backup unit, report if they are required.",
                "Officer, be aware that caller has been unable to be reached for some time, search the area and report.",
                "Showing you on scene investigating."
            };
        WCDispatchArriveS3 = activateAIBackup ?
            new string[]
            {
                "Officer, I'm showing you arrived on scene, backup on standby if required.",
                "Be on alert the caller advised that caller may be armed. Backup on standby.",
                "Showing you on scene. Report and return to patrol.",
                "Caller has requested a welfare check on a friend, proceed with caution.",
                "Showing you on scene, officer. Please report your findings, backup on standby.",
                "Backup in the area standing by, investigate and report officer.",
                "Backup in the area standing by, check the area and report your findings officer.",
                "We have you arriving on scene, back up is nearby.",
                "We have a unit on standby if you require it officer.",
                "Be aware, caller states they have been unable to reach welfare check individual.",
                "Copy that, showing you on scene. Backup is nearby if required.",
                "Investigate and report back officer. Backup unit standing by in the area."
            } :
            new string[]
            {
                "Officer, I'm showing you arrived on scene.",
                "Be on alert the caller advised that caller may be armed.",
                "Showing you on scene. Report and return to patrol.",
                "Caller has requested a welfare check on a friend, proceed with caution.",
                "Showing you on scene, officer. Please report your findings.",
                "Investigate and report back officer.",
                "Check the area and report your findings officer.",
                "We have you arriving on scene.",
                "Standing by for an update from you officer.",
                "Be aware, caller states they have been unable to reach welfare check individual.",
                "Copy that, showing you on scene.",
                "Locate individual and report back before returning to patrol, officer."
            };
        WCDispatchCode4 = activateAIBackup ?
            new string[]
            {
                "~w~All units, be advised, scene is code 4. No additional units required!",
                "~w~We are showing all units back on patrol.",
                "~w~Officer, you may all return to patrol.",
                "~w~Thank you officer, all units back available.",
                "~w~All units, code 4. Officer, you are welcome to stay although you should return to patrol.",
                "~w~10-4, Showing you all available."
            } :
            new string[]
            {
                "~w~Showing you code 4 on scene",
                "~w~You may stay on scene officer though you should return to patrol.",
                "~w~Copy that, showing you back on patrol.",
                "~w~Scene is clear, awaiting your next call.",
                "~w~Officer, you are required to return to patrol.",
                "~w~Code 4, All units back on patrol."
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
