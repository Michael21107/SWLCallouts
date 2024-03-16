// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.8.4

#region

#endregion

using LSPD_First_Response.Mod.API;

namespace SWLCallouts;

internal static class Settings
{
    private static readonly List<(bool enabled, Type callouts)> AllCallouts = new();
    // Check the department
    //internal static string Department = "SWLCO"; // Default to SWLCO logo //
    // Callouts below //
    internal static bool CyclistOnTheMotorway = true;
    internal static bool HighSpeedChase = true;
    internal static bool MurderInvestigation = true;
    internal static bool PersonWithAKnife = true;
    internal static bool ShotsFired = true;
    internal static bool StolenEmergencyVehicle = true;
    internal static bool StolenEmergencyVehicle2 = true;
    internal static bool WelfareCheck = true;
    // Extras below //
    internal static bool ActivateAIBackup = true;
    internal static bool HelpMessages = true;
    // Keys below //
    internal static Keys EndCall = Keys.End;
    internal static Keys Dialog = Keys.Y;

    internal static void LoadSettings()
    {
        // Read settings from INI file
        Settings("Checking SWLCallouts.ini exists...");
        var path = "Plugins/LSPDFR/SWLCallouts.ini";
        var ini = new InitializationFile(path);
        if (ini != null)
        {
            Settings("SWLCallouts.ini file NOT detected.");
            GameFiber.Sleep(500);
            Settings("Creating SWLCallouts.ini file...");
            ini.Create();
            GameFiber.Sleep(100);
            Settings("Writing default settings to SWLCallouts.ini file...");
            ini.Write("", "// File Created for SWLCallouts Settings by Scottywonderful", "");
            ini.Write("", "// Author: Scottywonderful", "");
            ini.Write("", $"// Created: {DateTime.Now.ToString("d MMM yyyy")}", ""); // Date of creation //
            ini.Write("", $"// Version: {PluginVersion}", ""); // Grabs current installed version //
            ini.Write("", "", "");
            ini.Write("[Callouts]", "[Callouts]", "");
            ini.Write("[Callouts]", "", "");
            ini.Write("[Callouts]", "// If true, the Callout is enabled.", "");
            ini.Write("[Callouts]", "// If false, the Callout is disabled. ", "");
            ini.Write("[Callouts]", "// You can disable callouts, if you do not want to have them in game. ", "");
            ini.Write("[Callouts]", "// (default -- true)", "");
            ini.Write("[Callouts]", "", "");
            ini.Write("[Callouts]", "CyclistOnTheMotorway", true);
            ini.Write("[Callouts]", "HighSpeedChase", true);
            ini.Write("[Callouts]", "PersonWithAKnife", true);
            ini.Write("[Callouts]", "ShotsFired", true);
            ini.Write("[Callouts]", "StolenEmergencyVehicle", true);
            ini.Write("[Callouts]", "StolenEmergencyVehicle2", true);
            ini.Write("[Callouts]", "WelfareCheck", true);
            ini.Write("[Callouts]", "", "");
            ini.Write("[Callouts]", "// The below are callouts which only work if you have 'Open All Interiors' installed.", "");
            ini.Write("[Callouts]", "// You can download 'Open All Interiors' here: https://www.gta5-mods.com/scripts/open-all-interiors", "");
            ini.Write("[Callouts]", "// (default -- false)", "");
            ini.Write("[Callouts]", "", false);
            ini.Write("[Callouts]", "", "");
            ini.Write("[Settings]", "[Settings]", "");
            ini.Write("[Settings]", "", "");
            ini.Write("[Settings]", "// This is experimental, DO NOT CHANGE UNLESS YOU KNOW WHAT YOU ARE DOING!", "");
            ini.Write("[Settings]", "// Choose between SWLCO, police, lssheriff, sheriff, highway, FIB, IAA, safire, saems/sams", "");
            ini.Write("[Settings]", "//Department = SWLCO", "");
            ini.Write("[Settings]", "", "");
            ini.Write("[Settings]", "// Activate this option to have AI units responding to certain callouts with the Player (you).", "");
            ini.Write("[Settings]", "// The backup type is different for each callout. This means you wont have a local unit responding to a heavily-armed terrorist attack.", "");
            ini.Write("[Settings]", "// (default -- true)", "");
            ini.Write("[Settings]", "ActivateAIBackup", true);
            ini.Write("[Settings]", "", "");
            ini.Write("[Settings]", "// This option allows you to remove the help messages used in some callouts and on startup.", "");
            ini.Write("[Settings]", "// (default -- true)", "");
            ini.Write("[Settings]", "HelpMessages", true);
            ini.Write("[Settings]", "", "");
            ini.Write("[Keys]", "[Keys]", "");
            ini.Write("[Keys]", "", "");
            ini.Write("[Keys]", "// You can change every key. Here is a list of valid keys you can use: https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx", "");
            ini.Write("[Keys]", "// With pressing this key, while you are in a callout of SWLCallouts, you can force the callout to end.", "");
            ini.Write("[Keys]", "EndCall", Keys.End);
            ini.Write("[Keys]", "", "");
            ini.Write("[Keys]", "// With pressing this key you can start the dialog, if there is a dialog in the callout.", "");
            ini.Write("[Keys]", "Dialog", Keys.Y);
            GameFiber.Sleep(500);
            Settings("Loading default settings...");
        }
        else
        {
            Settings("SWLCallouts.ini detected.");
            GameFiber.Sleep(200);
            Settings("Reading default settings...");
            GameFiber.Sleep(200);
            Settings("Loading custom settings...");
        }
        Settings("////////////////////////////////////////////////");
        Settings("////**********SWLCallouts Settings**********////");
        Settings("////////////////////////////////////////////////");

        // Callouts below //
        CyclistOnTheMotorway = ReadAndLogBoolean(ini, "SWLCyclistOnTheMotorway");
        HighSpeedChase = ReadAndLogBoolean(ini, "SWLHighSpeedChase");
        MurderInvestigation = ReadAndLogBoolean(ini, "SWLMurderInvestigation");
        PersonWithAKnife = ReadAndLogBoolean(ini, "SWLPersonWithAKnife");
        ShotsFired = ReadAndLogBoolean(ini, "SWLShotsFired");
        StolenEmergencyVehicle = ReadAndLogBoolean(ini, "SWLStolenEmergencyVehicle");
        StolenEmergencyVehicle2 = ReadAndLogBoolean(ini, "SWLStolenEmergencyVehicle2");
        WelfareCheck = ReadAndLogBoolean(ini, "SWLWelfareCheck");

        // Settings Below //
        //Department = ini.ReadString("Settings", "Department", "SWLCO"); // Default to SWLCallout Logo if not specified //
        ActivateAIBackup = ini.ReadBoolean("Settings", "ActivateAIBackup", true);
        HelpMessages = ini.ReadBoolean("Settings", "HelpMessages", true);

        // Keys Below //
        EndCall = ini.ReadEnum("Keys", "EndCall", Keys.End);
        Dialog = ini.ReadEnum("Keys", "Dialog", Keys.Y);
    }
    public static readonly string PluginVersion = "0.4.8.4";
    public static readonly string VersionType = "Alpha";

    private static bool ReadAndLogBoolean(InitializationFile ini, string calloutName)
    {
        bool enabled = ini.ReadBoolean("Callouts", calloutName, true);
        Settings($"{calloutName} = {enabled}");
        return enabled;
    }
}