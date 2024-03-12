// Author: Scottywonderful
// Created: 11th Mar 2024
// Version: 0.4.7.2

#region

#endregion

using Rage;

namespace SWLCallouts.Stuff;

internal class SWLCalloutHandler
{

    internal static void RegisterCallouts() // Register all callouts here//
    {
        Print("========================================== Start of callout loading for SWLCallouts ==========================================");
        if (Settings.CyclistOnTheMotorway) { Functions.RegisterCallout(typeof(SWLCyclistOnTheMotorway)); }
        if (Settings.HighSpeedChase) { Functions.RegisterCallout(typeof(SWLHighSpeedChase)); }
        //if (Settings.MurderInvestigation) { Functions.RegisterCallout(typeof(SWLMurderInvestigation)); }
        if (Settings.PersonWithAKnife) { Functions.RegisterCallout(typeof(SWLPersonWithAKnife)); }
        if (Settings.ShotsFired) { Functions.RegisterCallout(typeof(SWLShotsFired)); }
        if (Settings.StolenEmergencyVehicle) { Functions.RegisterCallout(typeof(SWLStolenEmergencyVehicle)); }
        if (Settings.StolenEmergencyVehicle2) { Functions.RegisterCallout(typeof(SWLStolenEmergencyVehicle2)); }
        if (Settings.WelfareCheck) { Functions.RegisterCallout(typeof(SWLWelfareCheck)); }
        Print("[LOG]: All callouts of the SWLCallouts.ini were loaded successfully.");
        Print("========================================== End of callout loading for SWLCallouts ==========================================");
    }

    internal static void DeregisterCallouts() // Unregister callouts due to unload/crash //
    {
        Log("Unregister all registered callouts");
        if (Settings.CyclistOnTheMotorway) { Functions.RegisterCallout(typeof(SWLCyclistOnTheMotorway)); }
        if (Settings.HighSpeedChase) { Functions.RegisterCallout(typeof(SWLHighSpeedChase)); }
        //if (Settings.MurderInvestigation) { Functions.RegisterCallout(typeof(SWLMurderInvestigation)); }
        if (Settings.PersonWithAKnife) { Functions.RegisterCallout(typeof(SWLPersonWithAKnife)); }
        if (Settings.ShotsFired) { Functions.RegisterCallout(typeof(SWLShotsFired)); }
        if (Settings.StolenEmergencyVehicle) { Functions.RegisterCallout(typeof(SWLStolenEmergencyVehicle)); }
        if (Settings.StolenEmergencyVehicle2) { Functions.RegisterCallout(typeof(SWLStolenEmergencyVehicle2)); }
        if (Settings.WelfareCheck) { Functions.RegisterCallout(typeof(SWLWelfareCheck)); }
        Print("[LOG]: All callouts of the SWLCallouts.ini were loaded successfully.");

        // TO BE ADDED
        // Add logic here to delete/dismiss all associated entities like peds and blips
        // Example:
        // - Dismiss Blips
        // - Delete peds
        // - Clear any other custom data such as vehicles and ped info.
        //if (_blip.Exists()) _blip.Delete();
        //if (_suspect.Exists()) _suspect.Dismiss();
        //if (_suspect.Exists()) _suspect.Dismiss();
        //if (_suspect.Exists()) _suspect.Dismiss();
        //if (_suspect.Exists()) _suspect.Dismiss();
        //if (_ped1.Exists()) Ped1.Dismiss();
        //if (_ped2.Exists()) Ped2.Dismiss();
        //if (_ped3.Exists()) Ped3.Dismiss();
        //if (_suspectVehicle.Exists()) _suspectVehicle.delete();
        //if (_emergencyVehicle.Exists()) _emergencyVehicle.delete();
        //if (_suspectBlip && _suspectBlip.Exists()) _suspectBlip.Delete();
    }
}
