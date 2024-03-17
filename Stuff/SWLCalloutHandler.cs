// Author: Scottywonderful
// Created: 11th Mar 2024
// Version: 0.4.8.6

#region

#endregion

using Rage;
using SWLCallouts.Callouts;

namespace SWLCallouts.Stuff;

internal class SWLCalloutHandler
{

    internal static void RegisterCallouts() // Register all callouts here//
    {
        Print("======================================== Start of callout loading for SWLCallouts ========================================");
        if (Settings.SWLCyclistOnTheMotorway) { Functions.RegisterCallout(typeof(SWLCyclistOnTheMotorway)); }
        if (Settings.SWLHighSpeedChase) { Functions.RegisterCallout(typeof(SWLHighSpeedChase)); }
        //if (Settings.MurderInvestigation) { Functions.RegisterCallout(typeof(SWLMurderInvestigation)); }
        if (Settings.SWLPersonWithAKnife) { Functions.RegisterCallout(typeof(SWLPersonWithAKnife)); }
        if (Settings.SWLShotsFired) { Functions.RegisterCallout(typeof(SWLShotsFired)); }
        if (Settings.SWLStolenEmergencyVehicle) { Functions.RegisterCallout(typeof(SWLStolenEmergencyVehicle)); }
        if (Settings.SWLStolenEmergencyVehicle2) { Functions.RegisterCallout(typeof(SWLStolenEmergencyVehicle2)); }
        if (Settings.SWLWelfareCheck) { Functions.RegisterCallout(typeof(SWLWelfareCheck)); }
        Print("[LOG]: All callouts of the SWLCallouts.ini were loaded successfully.");
        Print("========================================= End of callout loading for SWLCallouts =========================================");
    }

    internal static void DeregisterCallouts() // Unregister callouts due to unload/crash //
    {
        GameFiber.Sleep(200);
        Functions.StopCurrentCallout();
        Normal("Current callout was stopped (Including any not related to SWLCallouts).");
        Normal("Stopping all callouts and deleting blips/peds");
        Print("==================================== Stopping SWLCallouts ====================================");
        Print("");
        new SWLCyclistOnTheMotorway().End();
        Normal("CyclistOnTheMotorway Callout Entities Removed.");
        new SWLHighSpeedChase().End();
        Normal("HighSpeedChase Callout Entities Removed.");
        new SWLMurderInvestigation().End();
        Normal("MurderInvestigation Callout Entities Removed.");
        new SWLPersonWithAKnife().End();
        Normal("PersonWithAKnife Callout Entities Removed.");
        new SWLShotsFired().End();
        Normal("ShotsFired Callout Entities Removed.");
        new SWLStolenEmergencyVehicle().End();
        Normal("StolenEmergencyVehicle Callout Entities Removed.");
        new SWLStolenEmergencyVehicle2().End();
        Normal("StolenEmergencyVehicle2 Callout Entities Removed.");
        new SWLWelfareCheck().End();
        Normal("WelfareCheck Callout Entities Removed.");
        Print("");
        Normal("All SWL Callouts was stopped and all blips/peds removed.");
        Print("[LOG]: All callouts for SWLCallouts were unloaded successfully.");
        Print("");
        GameFiber.Sleep(200);
        Functions.StopCurrentCallout();
        Print("");
        Normal("Ensuring all callouts were stopped (Including any not related to SWLCallouts).");
        Print("[LOG]: Ensuring all callouts were stopped (Including any not related to SWLCallouts).");
        Print("============================== Callouts for SWLCallouts Stopped ==============================");

        /* Callout List
        CyclistOnTheMotorway
        HighSpeedChase
        MurderInvestigation
        PersonWithAKnife
        ShotsFired
        StolenEmergencyVehicle
        StolenEmergencyVehicle2
        WelfareCheck
        */
    }
}
