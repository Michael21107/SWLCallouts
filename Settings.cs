// Author: Scottywonderful
// Date: 16th Feb 2024  ||  Last Modified: 21st Feb 2024
// Version: 0.4.2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rage;
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
        internal static bool PersonWithKnife = true;
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
            HighSpeedChase = ini.ReadBoolean("Callouts", "SWLHighSpeedChase", true);
            WelfareCheck = ini.ReadBoolean("Callouts", "SWLWelfareCheck", true);
            // Settings Below //
            Department = ini.ReadString("Settings", "Department", "police"); // Default to police department if not specified //
            ActivateAIBackup = ini.ReadBoolean("Settings", "ActivateAIBackup", true);
            ActivateAIBackup = ini.ReadBoolean("Settings", "HelpMessages", true);
            // Keys Below //
            EndCall = ini.ReadEnum("Keys", "EndCall", Keys.End);
            Dialog = ini.ReadEnum("Keys", "Dialog", Keys.Y);
        }
        public static readonly string PluginVersion = "0.4.2.0";
        public static readonly string VersionType = "Alpha";
    }
}