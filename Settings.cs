// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.7.2

#region

#endregion

using LSPD_First_Response.Mod.API;

namespace SWLCallouts;

internal static class Settings
{
    // Check the department
    //internal static string Department = "SWLCO"; // Default to SWLCO logo //
    // Callouts below //
    internal static bool CyclistOnTheMotorway = true;
    internal static bool HighSpeedChase = true;
    //internal static bool MurderInvestigation = true;
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
        Log("[LOG]: Loading config file from SWLCallouts.");
        var ini = new InitializationFile(@"Plugins/LSPDFR/SWLCallouts.ini");
        ini.Create(); // Create an ini if one doesn't exist.
        // Callouts below //
        CyclistOnTheMotorway = ini.ReadBoolean("Callouts", "SWLCyclistOnTheMotorway", true);
        HighSpeedChase = ini.ReadBoolean("Callouts", "SWLHighSpeedChase", true);
        //MurderInvestigation = ini.ReadBoolean("Callouts", "SWLMurderInvestigation", true);
        PersonWithAKnife = ini.ReadBoolean("Callouts", "SWLPersonWithAKnife", true);
        ShotsFired = ini.ReadBoolean("Callouts", "SWLShotsFired", true);
        StolenEmergencyVehicle = ini.ReadBoolean("Callouts", "SWLStolenEmergencyVehicle", true);
        StolenEmergencyVehicle2 = ini.ReadBoolean("Callouts", "SWLStolenEmergencyVehicle2", true);
        WelfareCheck = ini.ReadBoolean("Callouts", "SWLWelfareCheck", true);
        // Settings Below //
        //Department = ini.ReadString("Settings", "Department", "SWLCO"); // Default to SWLCallout Logo if not specified //
        ActivateAIBackup = ini.ReadBoolean("Settings", "ActivateAIBackup", true);
        ActivateAIBackup = ini.ReadBoolean("Settings", "HelpMessages", true);
        // Keys Below //
        EndCall = ini.ReadEnum("Keys", "EndCall", Keys.End);
        Dialog = ini.ReadEnum("Keys", "Dialog", Keys.Y);
    }
    public static readonly string PluginVersion = "0.4.7.2";
    public static readonly string VersionType = "Beta";
}