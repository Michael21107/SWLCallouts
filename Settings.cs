// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.8.7

#region

#endregion

using LSPD_First_Response.Mod.API;
using System.IO;

namespace SWLCallouts;

internal static class Settings
{
    // Check the department
    //internal static string Department = "SWLCO"; // Default to SWLCO logo //
    // Callouts below //
    internal static bool SWLCyclistOnTheMotorway = true;
    internal static bool SWLHighSpeedChase = true;
    internal static bool SWLMurderInvestigation = false;
    internal static bool SWLPersonWithAKnife = true;
    internal static bool SWLShotsFired = true;
    internal static bool SWLStolenEmergencyVehicle = true;
    internal static bool SWLStolenEmergencyVehicle2 = true;
    internal static bool SWLWelfareCheck = true;
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
        if (!File.Exists(path))
        {
            Settings("SWLCallouts.ini file NOT detected.");
            GameFiber.Sleep(500);
            Settings("Creating SWLCallouts.ini file...");

            GameFiber.Sleep(100);
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            Settings("Writing default settings to SWLCallouts.ini file...");
            // Write default settings to the INI file
            WriteDefaultSettings(path);

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
        Settings("Callouts, Settings and Keybinds loading...");

        // Callouts below //
        Settings("..Callouts..");
        SWLCyclistOnTheMotorway = ini.ReadBoolean("Callouts", "CyclistOnTheMotorway", true);
        Settings($"CyclistOnTheMotorway = {SWLCyclistOnTheMotorway}");
        SWLHighSpeedChase = ini.ReadBoolean("Callouts", "HighSpeedChase", true);
        Settings($"HighSpeedChase = {SWLHighSpeedChase}");
        SWLMurderInvestigation = ini.ReadBoolean("Callouts", "MurderInvestigation", true);
        Settings($"MurderInvestigation = {SWLMurderInvestigation}");
        SWLPersonWithAKnife = ini.ReadBoolean("Callouts", "PersonWithAKnife", true);
        Settings($"PersonWithAKnife = {SWLPersonWithAKnife}");
        SWLShotsFired = ini.ReadBoolean("Callouts", "ShotsFired", true);
        Settings($"ShotsFired = {SWLShotsFired}");
        SWLStolenEmergencyVehicle = ini.ReadBoolean("Callouts", "StolenEmergencyVehicle", true);
        Settings($"StolenEmergencyVehicle = {SWLStolenEmergencyVehicle}");
        SWLStolenEmergencyVehicle2 = ini.ReadBoolean("Callouts", "StolenEmergencyVehicle2", true);
        Settings($"StolenEmergencyVehicle2 = {SWLStolenEmergencyVehicle2}");
        SWLWelfareCheck = ini.ReadBoolean("Callouts", "WelfareCheck", true);
        Settings($"WelfareCheck = {SWLWelfareCheck}");

        // Settings Below //
        Settings("..Settings..");
        //Department = ini.ReadString("Settings", "Department", "SWLCO"); // Default to SWLCallout Logo if not specified //
        ActivateAIBackup = ini.ReadBoolean("Settings", "ActivateAIBackup", true);
        Settings($"ActivateAIBackup = {ActivateAIBackup}");
        HelpMessages = ini.ReadBoolean("Settings", "HelpMessages", true);
        Settings($"HelpMessages = {HelpMessages}");

        // Keys Below //
        Settings("..Keybinds..");
        EndCall = ini.ReadEnum("Keys", "EndCall", Keys.End);
        Settings($"EndCall = {EndCall}");
        Dialog = ini.ReadEnum("Keys", "Dialog", Keys.Y);
        Settings($"Dialog = {Dialog}");
    }
    public static readonly string PluginVersion = "0.4.8.6";
    public static readonly string VersionType = "Alpha";

    private static void WriteDefaultSettings(string filePath)
    {
        using StreamWriter writer = new(filePath);
        Settings("Writing file title...");
        writer.WriteLine("// File Created by SWLCallouts Settings.");
        writer.WriteLine("// Author: Scottywonderful");
        writer.WriteLine($"// Created: {DateTime.Now:d MMM yyyy}"); // Date of creation //
        writer.WriteLine($"// Version: {PluginVersion}"); // Grabs current installed version //
        writer.WriteLine();
        // Callouts //
        Settings("Writing default callout settings...");
        writer.WriteLine("[Callouts]");
        writer.WriteLine();
        writer.WriteLine("// If true, the Callout is enabled.");
        writer.WriteLine("// If false, the Callout is disabled. ");
        writer.WriteLine("// You can disable callouts, if you do not want to have them in game. ");
        writer.WriteLine("// (default -- true)");
        writer.WriteLine();
        writer.WriteLine("CyclistOnTheMotorway = true");
        writer.WriteLine("HighSpeedChase = true");
        writer.WriteLine("PersonWithAKnife = true");
        writer.WriteLine("ShotsFired = true");
        writer.WriteLine("StolenEmergencyVehicle = true");
        writer.WriteLine("StolenEmergencyVehicle2 = true");
        writer.WriteLine("WelfareCheck = true");
        writer.WriteLine();
        writer.WriteLine("// The below are callouts which only work if you have 'Open All Interiors' installed.");
        writer.WriteLine("// You can download 'Open All Interiors' here: https://www.gta5-mods.com/scripts/open-all-interiors");
        writer.WriteLine("// (default -- false)");
        writer.WriteLine("MurderInvestigation = false");
        writer.WriteLine();
        // Settings //
        Settings("Writing default settings...");
        writer.WriteLine("[Settings]");
        writer.WriteLine();
        writer.WriteLine("// This is experimental, DO NOT CHANGE UNLESS YOU KNOW WHAT YOU ARE DOING!");
        writer.WriteLine("// Choose between SWLCO, police, lssheriff, sheriff, highway, FIB, IAA, safire, saems/sams");
        writer.WriteLine("//Department = SWLCO");
        writer.WriteLine();
        writer.WriteLine("// Activate this option to have AI units responding to certain callouts with the Player (you).");
        writer.WriteLine("// The backup type is different for each callout. This means you wont have a local unit responding to a heavily-armed terrorist attack.");
        writer.WriteLine("// (default -- true)");
        writer.WriteLine("ActivateAIBackup = true");
        writer.WriteLine();
        writer.WriteLine("// This option allows you to remove the help messages used in some callouts and on startup.");
        writer.WriteLine("// (default -- true)");
        writer.WriteLine("HelpMessages = true");
        writer.WriteLine();
        // Keys //
        Settings("Writing default keys...");
        writer.WriteLine("[Keys]");
        writer.WriteLine();
        writer.WriteLine("// You can change every key. Here is a list of valid keys you can use: https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx");
        writer.WriteLine("// With pressing this key, while you are in a callout of SWLCallouts, you can force the callout to end.");
        writer.WriteLine("EndCall = End");
        writer.WriteLine();
        writer.WriteLine("// With pressing this key you can start the dialog, if there is a dialog in the callout.");
        writer.WriteLine("Dialog = Y");
        // FILE CREATED //
        Settings("Default SWLCallouts.ini file written to location.");
    }
}