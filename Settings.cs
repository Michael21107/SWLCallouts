// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.5.1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rage;
using SWLCallouts.Callouts;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SWLCallouts
{
    internal static class Settings
    {
        // Check the department
        internal static string Department = "police"; // Default to police department //
        // Callouts below //
        internal static bool ApartmentBurglary = true;
        internal static bool ArmedClown = true;
        internal static bool ArmedTerroristAttack = true;
        internal static bool BicycleOnTheFreeway = true;
        internal static bool DrugDeal = true;
        internal static bool GangShootout = true;
        internal static bool HighSpeedChase = true;
        internal static bool HostageSituationReported = true;
        internal static bool IllegalPoliceCarTrade = true;
        internal static bool JewelleryRobbery = true;
        internal static bool K9BackupRequired = true;
        internal static bool MoneyTruckTheft = true;
        internal static bool MurderInvestigation = true;
        internal static bool PersonWithAKnife = true;
        internal static bool PublicPeaceDisturbance = true;
        internal static bool RobberyHL = true;
        internal static bool ShotsFired = true;
        internal static bool StolenBusIncident = true;
        internal static bool StolenEmergencyVehicle = true;
        internal static bool StolenEmergencyVehicle2 = true;
        internal static bool StolenTruckPursuit = true;
        internal static bool StoreRobberyInProgress = true;
        internal static bool SuspiciousATMActivity = true;
        internal static bool TrafficStopBackupRequired = true;
        internal static bool Troublemaker = true;
        internal static bool WarrantForArrest = true;
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
            Game.LogTrivial("[LOG]: Loading config file from SWLCallouts.");
            var path = "Plugins/LSPDFR/SWLCallouts.ini";
            var ini = new InitializationFile(path);
            ini.Create();
            // Callouts below //
            ApartmentBurglary = ini.ReadBoolean("Callouts", "SWLApartmentBurglary", true);
            ArmedClown = ini.ReadBoolean("Callouts", "SWLArmedClown", true);
            ArmedTerroristAttack = ini.ReadBoolean("Callouts", "SWLArmedTerroristAttack", true);
            BicycleOnTheFreeway = ini.ReadBoolean("Callouts", "SWLBicycleOnTheFreeway", true);
            DrugDeal = ini.ReadBoolean("Callouts", "SWLDrugDeal", true);
            GangShootout = ini.ReadBoolean("Callouts", "SWLGangShootout", true);
            HighSpeedChase = ini.ReadBoolean("Callouts", "SWLHighSpeedChase", true);
            HostageSituationReported = ini.ReadBoolean("Callouts", "SWLHostageSituationReported", true);
            IllegalPoliceCarTrade = ini.ReadBoolean("Callouts", "SWLIllegalPoliceCarTrade", true);
            JewelleryRobbery = ini.ReadBoolean("Callouts", "SWLJewelleryRobbery", true);
            K9BackupRequired = ini.ReadBoolean("Callouts", "SWLK9BackupRequired", true);
            MoneyTruckTheft = ini.ReadBoolean("Callouts", "SWLMoneyTruckTheft", true);
            MurderInvestigation = ini.ReadBoolean("Callouts", "SWLMurderInvestigation", true);
            PersonWithAKnife = ini.ReadBoolean("Callouts", "SWLPersonWithAKnife", true);
            PublicPeaceDisturbance = ini.ReadBoolean("Callouts", "SWLPublicPeaceDisturbance", true);
            RobberyHL = ini.ReadBoolean("Callouts", "SWLRobberyHL", true);
            ShotsFired = ini.ReadBoolean("Callouts", "SWLShotsFired", true);
            StolenBusIncident = ini.ReadBoolean("Callouts", "SWLStolenBusIncident", true);
            StolenEmergencyVehicle = ini.ReadBoolean("Callouts", "SWLStolenEmergencyVehicle", true);
            StolenEmergencyVehicle2 = ini.ReadBoolean("Callouts", "SWLStolenEmergencyVehicle2", true);
            StolenTruckPursuit = ini.ReadBoolean("Callouts", "SWLStolenTruckPursuit", true);
            StoreRobberyInProgress = ini.ReadBoolean("Callouts", "SWLStoreRobberyInProgress", true);
            SuspiciousATMActivity = ini.ReadBoolean("Callouts", "SWLSuspiciousATMActivity", true);
            TrafficStopBackupRequired = ini.ReadBoolean("Callouts", "SWLTrafficStopBackupRequired", true);
            Troublemaker = ini.ReadBoolean("Callouts", "SWLTroublemaker", true);
            WarrantForArrest = ini.ReadBoolean("Callouts", "SWLWarrantForArrest", true);
            WelfareCheck = ini.ReadBoolean("Callouts", "SWLWelfareCheck", true);
            // Settings Below //
            Department = ini.ReadString("Settings", "Department", "police"); // Default to police department if not specified //
            ActivateAIBackup = ini.ReadBoolean("Settings", "ActivateAIBackup", true);
            ActivateAIBackup = ini.ReadBoolean("Settings", "HelpMessages", true);
            // Keys Below //
            EndCall = ini.ReadEnum("Keys", "EndCall", Keys.End);
            Dialog = ini.ReadEnum("Keys", "Dialog", Keys.Y);
        }
        public static readonly string PluginVersion = "0.4.5.1";
        public static readonly string VersionType = "Alpha";
    }
}